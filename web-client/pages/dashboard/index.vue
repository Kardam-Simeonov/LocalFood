<template>
    <main>
        <section class="grid grid-cols-12 xl:h-[85vh] overflow-y-hidden">
            <div ref="mapRef" class="xl:col-span-8 col-span-full h-[60rem]"></div>
            <aside
                class="xl:col-span-4 col-span-full bg-red-500 xl:h-[85vh] overflow-y-auto px-8 py-12 flex flex-col">
                <h1 class="text-4xl text-white font-semibold mb-12">Your orders</h1>
                <div v-for="(order, index) in userOrders" :key="index" class="bg-white shadow-lg rounded-lg p-6 mb-6">
                    <div class="font-bold text-xl mb-4">Order #{{ order.id }}</div>
                    <div v-for="(orderProduct, index) in order.orderProducts" :key="index"
                        class="flex items-center mb-4">
                        <img :src="'data:image/jpeg;base64,' + orderProduct.product.image"
                            class="w-16 h-16 object-cover rounded-lg mr-4">
                        <div>
                            <div class="font-bold">{{ orderProduct.product.name }}</div>
                            <div class="text-gray-600">${{ orderProduct.product.price.toFixed(2) }}</div>
                        </div>
                    </div>
                    <p class="font-semibold">Deliver to: {{ order.address }}</p>
                    <button @click="updateOrder(order.id)"
                        class="bg-green-500 text-white px-4 py-2 rounded-lg hover:bg-green-600 focus:outline-none focus:ring-2 focus:ring-green-500 focus:ring-opacity-50 mt-4">Mark
                        as ready for pickup</button>
                </div>
                <div v-if="userOrders.length === 0"
                    class="border-dashed border-4 border-red-800 rounded-lg p-4 flex flex-col justify-center items-center min-h-[15rem]">
                    <p class="text-white text-center">You have no orders</p>
                </div>
            </aside>
        </section>
        <section class="py-12 text-white">
            <h1 class="text-4xl font-bold text-center mb-16 text-red-700">Your Products</h1>
            <div class="grid lg:grid-cols-3 md:grid-cols-2 grid-cols-1 gap-4 max-w-5xl px-4 mx-auto">
                <div v-for="(product, index) in userProducts" :key="index"
                    class="bg-red-500 rounded-lg p-4 flex flex-col justify-between relative">
                    <img :src="'data:image/jpeg;base64,' + product.image" alt="Product Image"
                        class="w-full h-56 object-cover rounded-lg mb-6">
                    <h2 class="text-xl font-bold">{{ product.name }}</h2>
                    <p class="text-lg font-semibold mt-14 mb-4">${{ product.price.toFixed(2) }}</p>
                    <div class="absolute bottom-4 right-4 flex gap-2">
                        <NuxtLink :to="'/dashboard/edit/' + product.id">
                            <button
                                class="bg-red-700 text-white px-4 py-2 rounded-lg hover:bg-red-600 focus:outline-none focus:ring-2 focus:ring-red-500 focus:ring-opacity-50">Edit</button>
                        </NuxtLink>
                        <button @click="deleteProduct(product.id)"
                            class="bg-red-700 text-white px-4 py-2 rounded-lg hover:bg-red-600 focus:outline-none focus:ring-2 focus:ring-red-500 focus:ring-opacity-50">Delete</button>
                    </div>
                </div>
                <NuxtLink to="/dashboard/add">
                    <div
                        class="h-full border-dashed border-4 border-red-500 hover:border-red-300 text-red-500 hover:text-red-300 cursor-pointer rounded-lg p-4 flex flex-col justify-center items-center min-h-[23rem]">
                        <Icon name="bi:plus-circle" class="text-6xl" />
                    </div>
                </NuxtLink>
            </div>
        </section>
    </main>
</template>

<script setup>
import Map from 'ol/Map';
import View from 'ol/View';
import TileLayer from 'ol/layer/Tile';
import OSM from 'ol/source/OSM';
import Feature from 'ol/Feature';
import Point from 'ol/geom/Point';
import VectorLayer from 'ol/layer/Vector';
import VectorSource from 'ol/source/Vector';
import { Circle as CircleStyle, Fill, Stroke, Style } from 'ol/style';
import { fromLonLat } from 'ol/proj';

const userId = useCookie('userId');

definePageMeta({
    layout: 'vendor',
    middleware: 'auth'
})

const { data: productsData } = await useFetch(`https://localhost:7230/api/products`);

const userProducts = ref([]);

userProducts.value = productsData.value.filter(product => product.vendorId === userId.value)

const { data: ordersData } = await useFetch(`https://localhost:7230/api/orders/vendor/${userId.value}`);

const userOrders = ref([]);

userOrders.value = ordersData.value.filter(order => 
    order.orderProducts.length > 0 && !order.orderProducts[0].readyForPickup
);

console.log(ordersData.value);

const deleteProduct = async (id) => {
    try {
        await $fetch(`https://localhost:7230/api/products/${id}`, {
            method: 'DELETE',
        });
        // Remove the deleted product from the userProducts array
        userProducts.value = userProducts.value.filter(product => product.id !== id);
    } catch (error) {
        console.log(error);
    }
};

const updateOrder = async (id) => {
    try {
        // For each order product in the order, update the product's readyForPickup status
        const order = userOrders.value.find(order => order.id === id);

        for (let orderProduct of order.orderProducts) {
            console.log(orderProduct.readyForPickup);
            await $fetch(`https://localhost:7230/api/orders/${order.id}/product/${orderProduct.id}`, {
                method: 'PUT',
                body: {
                    readyForPickup: true
                }
            });
        }

        // Remove the order from the userOrders array
        userOrders.value = userOrders.value.filter(order => order.id !== id);
        removeMarker(id);
    } catch (error) {
        console.log(error);
    }
};

const mapRef = ref(null);
let map;
let vectorSource;

onMounted(() => {
    map = new Map({
        target: mapRef.value,
        layers: [
            new TileLayer({
                source: new OSM(),
            })
        ],
        view: new View({
            center: fromLonLat([23.3219, 42.6975]),
            zoom: 12
        }),
        controls: [],
    });

    vectorSource = new VectorSource();
    const vectorLayer = new VectorLayer({
        source: vectorSource,
        style: new Style({
        image: new CircleStyle({
          radius: 10,
          fill: new Fill({
            color: '#a5f3fc',
          }),
          stroke: new Stroke({
            color: 'white',
            width: 1,
            opacity: 0.1
          }),
        }),
      })
    });
    map.addLayer(vectorLayer);

    // Add markers for existing orders
    userOrders.value.forEach(order => {
        addMarker(order);
    });
});

onUnmounted(() => {
    // Clean up resources when the component is unmounted
    map = null;
    vectorSource = null;
});

const addMarker = (order) => {
    const orderLocation = fromLonLat([order.longitude, order.latitude]);
    const marker = new Feature({
        geometry: new Point(orderLocation)
    });
    vectorSource.addFeature(marker);
    map.set(order.id, marker); // Associate marker with order ID
};

const removeMarker = (orderId) => {
    const marker = map.get(orderId);
    if (marker) {
        vectorSource.removeFeature(marker);
    }
};
</script>