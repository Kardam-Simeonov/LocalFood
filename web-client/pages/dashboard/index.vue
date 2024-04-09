<template>
    <main>
        <section class="grid grid-cols-12 xl:h-[90vh]">
            <div id="map" ref="mapRef" class="xl:col-span-8 col-span-full xl:h-full h-[50vh]"></div>
            <aside
                class="xl:col-span-4 col-span-full bg-red-500 xl:h-full h-[70vh] overflow-y-auto px-8 py-12 flex flex-col">
                <h1 class="text-4xl text-white font-semibold mb-12">Your orders</h1>
                <div v-for="(order, index) in userOrders" :key="index" class="bg-white shadow-lg rounded-lg p-6 mb-6">
                    <div class="font-bold text-xl mb-4">Order #{{ order.id }}</div>
                        <div v-for="(orderProduct, index) in order.orderProducts" :key="index" class="flex items-center mb-4">
                            <img :src="'data:image/jpeg;base64,' + orderProduct.product.image" class="w-16 h-16 object-cover rounded-lg mr-4">
                            <div>
                                <div class="font-bold">{{ orderProduct.product.name }}</div>
                                <div class="text-gray-600">${{ orderProduct.product.price.toFixed(2) }}</div>
                            </div>
                        </div>
                    <p class="font-semibold">Deliver to: {{ order.address }}</p>
                    <button @click="deleteOrder(order.id)" class="bg-green-500 text-white px-4 py-2 rounded-lg hover:bg-green-600 focus:outline-none focus:ring-2 focus:ring-green-500 focus:ring-opacity-50 mt-4">Mark as ready for pickup</button>
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
                <div v-for="(product, index) in userProducts" :key="index" class="bg-red-500 rounded-lg p-4 flex flex-col justify-between relative">
                    <img :src="'data:image/jpeg;base64,' + product.image" alt="Product Image" class="w-full h-56 object-cover rounded-lg mb-6">
                    <h2 class="text-xl font-bold">{{ product.name }}</h2>
                    <p class="text-lg font-semibold mt-14 mb-4">${{ product.price.toFixed(2) }}</p>
                    <button @click="deleteProduct(product.id)" class="absolute bottom-4 right-4 bg-red-700 text-white px-4 py-2 rounded-lg hover:bg-red-600 focus:outline-none focus:ring-2 focus:ring-red-500 focus:ring-opacity-50">Delete</button>
                </div>
                <NuxtLink to="/dashboard/add">
                    <div class="h-full border-dashed border-4 border-red-500 hover:border-red-300 text-red-500 hover:text-red-300 cursor-pointer rounded-lg p-4 flex flex-col justify-center items-center min-h-[23rem]">
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
import { fromLonLat } from 'ol/proj';

const userId = useCookie('userId');

definePageMeta({
  layout: 'vendor',
  middleware: 'auth'
})

const { data: productsData } = await useFetch(`https://localhost:7230/api/products`);

const userProducts = ref([]);

userProducts.value = productsData.value.filter(product => product.vendorId === userId.value)

console.log(userProducts.value);

const { data: ordersData } = await useFetch(`https://localhost:7230/api/orders/vendor/${userId.value}`);

const userOrders = ref([]);

userOrders.value = ordersData.value;

console.log(ordersData.value);

const deleteProduct = async (id) => {
try{
  await $fetch(`https://localhost:7230/api/products/${id}`, {
    method: 'DELETE',
  });
  // Remove the deleted product from the userProducts array
  userProducts.value = userProducts.value.filter(product => product.id !== id);
} catch (error){
    console.log(error);
}
};

const deleteOrder = async (id) => {
try{
  await $fetch(`https://localhost:7230/api/orders/${id}`, {
    method: 'DELETE',
  });
  // Remove the deleted product from the userProducts array
  userOrders.value = userOrders.value.filter(order => order.id !== id);
} catch (error){
    console.log(error);
}
};

console.log(userProducts.value)

const mapRef = ref(null);

onMounted(() => {
    const map = new Map({
        target: mapRef.value,
        layers: [
            new TileLayer({
                source: new OSM()
            })
        ],
        view: new View({
            center: fromLonLat([23.3219, 42.6975]),
            zoom: 12
        }),
        controls: [],
    });
});
</script>