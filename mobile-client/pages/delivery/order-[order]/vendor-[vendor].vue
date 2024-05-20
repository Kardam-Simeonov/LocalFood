<template>
    <div class="relative flex flex-col overflow-hidden h-screen bg-quakeGreen-background">
        <div id="map" ref="mapRef" class="w-full h-screen" :class="{ 'invisible': isLoading }"></div>
        <div id="overlay" ref="overlayRef" class="relative" :class="{ 'invisible': isLoading }">
            <div class="flex flex-col relative z-10 bg-white text-sm rounded-lg p-2 w-[12rem]">
                <p class="flex gap-3">
                    <Icon :icon="overlayContent.deliveryInformation.icon" class="my-auto" /> {{
            overlayContent.deliveryInformation.addressType }}
                </p>
                <p class="ml-10">{{ overlayContent.deliveryInformation.address }}</p>
            </div>
            <div class="w-0 h-0 absolute z-0 -bottom-2 left-0 right-0 mx-auto
                        border-l-[40px] border-l-transparent
                        border-t-[35px] border-t-white
                        border-r-[40px] border-r-transparent">
            </div>
        </div>
        <section
            class="absolute h-[50rem] flex flex-col w-full bg-red-500 bg-opacity-95 rounded-t-[2.5rem] transition-transform duration-500 border-t-4 border-red-400"
            :style="{ transform: !isCardVisible ? 'translateY(calc(100vh - 8rem))' : 'translateY(calc(100vh - 45rem))' }">
            <div class="relative flex p-6 gap-2" :class="{ 'invisible': isLoading }"
                @click="isCardVisible = !isCardVisible">
                <div class="flex flex-col justify-center">
                    <h1
                        class="font-semibold mb-1 ml-2 text-2xl whitespace-normal break-normal text-white drop-shadow-md">
                        Order #4032</h1>
                    <p class="ml-2 truncate text-quakeGreen text-sm flex text-white">
                        <Icon icon="fa6-solid:location-arrow" class="my-auto mr-3" />Deliver to: {{ currentOrder.address
                        }}
                    </p>
                </div>
                <Icon class="ml-auto w-12 mb-1 text-4xl text-white transition-transform duration-500 drop-shadow-md"
                    icon="fa6-solid:chevron-up" :class="{ 'rotate-180': isCardVisible }" />
            </div>
            <ul class="h-[39rem] overflow-x-hidden overflow-y-auto bg-white text-opacity-90 mx-3 px-4 pt-6 pb-14 flex flex-col gap-2 rounded-lg text-black"
                :class="{ 'invisible': isLoading }">
                <li class="mb-4 font-semibold text-xl">Delivery Information</li>
                <div class="ml-2">
                    <div v-for="(orderProduct, index) in currentOrder.orderProducts" :key="index"
                        class="flex items-center mb-4">
                        <img :src="'data:image/jpeg;base64,' + orderProduct.product.image"
                            class="w-16 h-16 object-cover rounded-lg mr-4">
                        <div>
                            <div class="font-bold">{{ orderProduct.product.name }}</div>
                            <div class="text-gray-600">${{ orderProduct.product.price.toFixed(2) }}</div>
                        </div>
                    </div>
                </div>
                <li>
                    <h2 class="font-semibold text-lg">Contact Details</h2>
                </li>
                <li class="flex flex-col gap-1">
                    <p class="flex gap-3">
                        <Icon icon="fa6-solid:user" class="my-auto" /> Client Details
                    </p>
                    <p class="ml-10">{{ currentOrder.fullName }}</p>
                    <p class="ml-10">{{ currentOrder.phoneNumber }}</p>
                </li>
                <li class="flex flex-col gap-1">
                    <p class="flex gap-3">
                        <Icon icon="fa6-solid:user" class="my-auto" /> Vendor Details
                    </p>
                    <p class="ml-10">{{ vendor.name }}</p>
                    <p class="ml-10">{{ vendor.phoneNumber }}</p>
                </li>
                <li>
                    <h2 class="font-semibold text-lg">Order Details</h2>
                </li>
                <li class="flex flex-col gap-1">
                    <p class="flex gap-3">
                        <Icon icon="fa6-solid:truck-arrow-right" class="my-auto" /> Pickup Address
                    </p>
                    <p class="ml-10">{{ vendor.address }}</p>
                </li>
                <li class="flex flex-col gap-1">
                    <p class="flex gap-3">
                        <Icon icon="fa6-solid:location-crosshairs" class="my-auto" /> Delivery Address
                    </p>
                    <p class="ml-10">{{ currentOrder.address }}</p>
                    <p v-if="currentOrder.floor != ''" class="ml-10">Floor - {{ currentOrder.floor }}</p>
                    <p v-if="currentOrder.apartment != ''" class="ml-10">Apartment - {{ currentOrder.apartment }}</p>
                    <p v-if="currentOrder.entrance != ''" class="ml-10">Entrance - {{ currentOrder.entrance }}</p>
                </li>
                <li class="flex flex-col gap-1">
                    <p class="flex gap-3">
                        <Icon icon="fa6-solid:note-sticky" class="my-auto" /> Delivery Note
                    </p>
                    <p class="ml-10">{{ currentOrder.deliveryNote == '' ? `This order doesn't have a delivery note` :
            currentOrder.deliveryNote }}</p>
                </li>
            </ul>
            <div class="flex" :class="{ 'invisible': isLoading }">
                <div class="w-full h-[0.15rem] my-4 rounded-full mx-24 bg-quakeGreen opacity-30"></div>
            </div>
        </section>
        <header class="absolute flex w-full h-[6rem] px-4 py-4">
            <RouterLink class="w-full" to="/">
                <button type="button"
                    class="bg-red-500 text-white text-sm font-bold h-full w-full rounded-l-2xl p-3 border-x-2 border-y-2 border-red-400">
                    Cancel Order
                </button>
            </RouterLink>
            <button @click="completeOrder()"
                class="bg-green-500 text-white text-sm font-bold h-full w-full rounded-r-2xl p-3 border-x-2 border-y-2 border-green-400">
                Complete Order
            </button>
        </header>
    </div>
</template>

<script setup>
// Import the needed libraries
import { ref, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import { Icon } from '@iconify/vue'
import { Geolocation } from '@capacitor/geolocation';
import Map from 'ol/Map';
import OSM from 'ol/source/OSM';
import View from 'ol/View';
import TileLayer from 'ol/layer/Tile';
import VectorLayer from 'ol/layer/Vector';
import VectorSource from 'ol/source/Vector';
import GeoJSON from 'ol/format/GeoJSON';
import Overlay from 'ol/Overlay';
import { RegularShape } from 'ol/style';
import Point from 'ol/geom/Point';
import Feature from 'ol/Feature';
import { Circle as CircleStyle, Fill, Stroke, Style } from 'ol/style';
import { fromLonLat } from 'ol/proj';

definePageMeta({
    middleware: 'auth'
})

// const route = useRoute();
// const quakeId = ref(route.params.id);
const isLoading = ref(true);
const mapRef = ref(null);
const isCardVisible = ref(false);
const overlayRef = ref(null);
const overlayContent = ref({
    deliveryInformation: {
        icon: '',
        addressType: '',
        address: '',
    }
});

const route = useRoute();
const router = useRouter();
console.log(route.params.vendor)


const { data: orderData } = await useFetch(`https://localhost:7230/api/orders/${route.params.order}`);
const currentOrder = ref([]);
currentOrder.value = orderData.value;
currentOrder.value.orderProducts = currentOrder.value.orderProducts.filter(orderProduct => orderProduct.product.vendorId == route.params.vendor);

const { data: vendor } = await useFetch(`https://localhost:7230/api/auth/vendor/${route.params.vendor}`);

async function completeOrder() {
    for (let orderProduct of currentOrder.value.orderProducts) {
        console.log(orderProduct);
        try {
            await $fetch(`https://localhost:7230/api/orders/${route.params.order}/product/${orderProduct.id}`, {
                method: 'DELETE',
            });
        } catch (error) {
            console.log(error);
        }
    }

    const order = await $fetch(`https://localhost:7230/api/orders/${route.params.order}`);

    if (order.orderProducts.length == 0) {
        try {
            await $fetch(`https://localhost:7230/api/orders/${route.params.order}`, {
                method: 'DELETE',
            });
        } catch (error) {
            console.log(error);
        }
    }

    router.push('/');
}


// The onMounted hook runs when the component is mounted to the website DOM,
// it allows us to access the DOM (HTML) elements and initialize the map
onMounted(() => {
    const currentPosition = async () => {
        return await Geolocation.getCurrentPosition();
    }

    // When the current position is retrieved, we can initialize the map
    currentPosition().then(position => {
        const map = new Map({
            // Reference the mapRef variable in the template to set the map target
            target: mapRef.value,
            // The layers array holds the map data and markers
            layers: [
                // Create a new map tile layer from the OpenStreetMap source
                new TileLayer({
                    source: new OSM()
                }),
            ],
            // The view defines the center and zoom level of the map
            // We also use the current position of the user to center the map,
            // and remove any default controls (zoom, rotate, etc.)
            view: new View({
                center: fromLonLat([position.coords.longitude, position.coords.latitude]),
                zoom: 12,
            }),
            controls: []
        });

        // Create a new vector layer for the user feedback markers
        const deliveryLocationsLayer = new VectorLayer({
            source: new VectorSource(),
            style: new Style({
                image: new RegularShape({
                    fill: new Fill({
                        color: '#93c5fd',
                    }),
                    stroke: new Stroke({
                        color: 'white',
                        width: 2,
                    }),
                    points: 3,
                    radius: 10,
                    rotation: Math.PI, // rotate the triangle by 180 degrees ;)
                }),
            }),
        });

        // Create a new source for vector markers,
        // and add a marker at the current position of the user
        const markerSource = new VectorSource();
        const marker = new Feature({
            geometry: new Point(fromLonLat([position.coords.longitude, position.coords.latitude])),
        });
        markerSource.addFeature(marker);

        // Create a new layer for the marker,
        // and add it to the map
        const markerLayer = new VectorLayer({
            source: markerSource,
        });

        map.addLayer(markerLayer);

        console.log([currentOrder.value.latitude, currentOrder.value.longitude]);
        console.log([vendor.value.latitude, vendor.value.longitude]);
        const feature1 = new Feature({
            geometry: new Point(fromLonLat([currentOrder.value.longitude, currentOrder.value.latitude])),
            deliveryInformation: {
                icon: 'fa6-solid:truck-arrow-right',
                addressType: 'Delivery Address',
                address: currentOrder.value.address,
            }
        });
        const feature2 = new Feature({
            geometry: new Point(fromLonLat([vendor.value.longitude, vendor.value.latitude])),
            deliveryInformation: {
                icon: 'fa6-solid:truck-arrow-right',
                addressType: 'Pickup Address',
                address: vendor.value.address,
            }
        });

        deliveryLocationsLayer.getSource().addFeatures([feature1, feature2]);

        // Add the offset marker layer to the map
        map.addLayer(deliveryLocationsLayer);

        // Create a new overlay instance, which will display the earthquake info,
        // when a marker is clicked
        const overlay = new Overlay({
            element: overlayRef.value,
            positioning: 'bottom-center',
            offset: [0, -10],
        });

        // Add the instance to the map
        map.addOverlay(overlay);

        // When a marker is clicked, get its properties and display them in the overlay
        map.on('singleclick', (event) => {
            // Hide the overlay by default,
            // in case the user clicks on the map, instead of a marker
            overlay.setPosition(undefined);

            // Get the features (markers) at the clicked position
            map.forEachFeatureAtPixel(event.pixel, (feature) => {
                // If there is a feature found at the clicked position
                if (feature) {
                    // Center the map view on the clicked feature
                    const view = map.getView();
                    view.animate({
                        center: feature.getGeometry().getCoordinates(),
                        duration: 1000
                    });

                    // Get the feature properties
                    const { deliveryInformation } = feature.getProperties();

                    // If there is no feltScore or damageScore,
                    // we don't want to display the overlay
                    if (!deliveryInformation)
                        return;

                    // Display the overlay and set its content
                    overlay.setPosition(feature.getGeometry().getCoordinates());
                    overlayContent.value.deliveryInformation = deliveryInformation;
                }
            });
        });

        isLoading.value = false;
    });
});
</script>
