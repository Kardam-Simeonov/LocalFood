<template>
    <div class="relative flex flex-col overflow-hidden h-screen bg-quakeGreen-background">
        <div id="map" ref="mapRef" class="w-full h-screen" :class="{ 'invisible': isLoading }"></div>
        <div id="overlay" ref="overlayRef" class="relative" :class="{ 'invisible': isLoading }">
            <div class="flex flex-col relative z-10 bg-white text-quakeGreen-dark text-xs rounded-lg p-2 w-[12rem]">
                <h1 class="text-sm font-semibold text-quakeGreen">Почувствано:</h1>
                <div class="flex gap-2 pl-2">
                    <p class="flex items-center">- {{ overlayContent.feltScore.description }}</p>
                    <p class="flex items-center justify-center aspect-square rounded-full text-center text-base p-1 w-8 ml-auto font-bold text-white shadow"
                        :style="{ 'background-color': getScoreColorClass(overlayContent.feltScore.value + 1) }">
                        {{ overlayContent.feltScore.value }}
                    </p>
                </div>
                <h1 class="text-sm font-semibold text-quakeGreen border-t-2 mt-2 pt-2">Поражение:</h1>
                <div class="flex gap-2 pl-2">
                    <p class="flex items-center">- {{ overlayContent.damageScore.description }}</p>
                    <p class="flex items-center justify-center aspect-square rounded-full text-center text-base p-1 w-8 ml-auto font-bold text-white shadow"
                        :style="{ 'background-color': getScoreColorClass(overlayContent.damageScore.value + 1) }">
                        {{ overlayContent.damageScore.value }}
                    </p>
                </div>
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
                        <Icon icon="fa6-solid:location-arrow" class="my-auto mr-3" />Deliver to: 5006 2, Sofia
                    </p>
                </div>
                <Icon class="ml-auto w-12 mb-1 text-4xl text-white transition-transform duration-500 drop-shadow-md"
                    icon="fa6-solid:chevron-up" :class="{ 'rotate-180': isCardVisible }" />
            </div>
            <ul class="h-[39rem] overflow-x-hidden overflow-y-auto bg-white text-opacity-90 mx-3 px-4 pt-6 pb-14 flex flex-col gap-2 rounded-lg text-gray-700"
                :class="{ 'invisible': isLoading }">
                <li class="mb-4 font-semibold text-xl">Delivery Information</li>
                <div v-for="(order, index) in userOrders" :key="index" class="ml-2">
                    <div v-for="(orderProduct, index) in order.orderProducts" :key="index"
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
                    <p class="ml-10">Kardam Simeonov</p>
                    <p class="ml-10">+359 XXXXXXXX</p>
                </li>
                <li class="flex flex-col gap-1">
                    <p class="flex gap-3">
                        <Icon icon="fa6-solid:user" class="my-auto" /> Vendor Details
                    </p>
                    <p class="ml-10">Sali Yashar</p>
                    <p class="ml-10">+359 XXXXXXXX</p>
                </li>
                <li>
                    <h2 class="font-semibold text-lg">Order Details</h2>
                </li>
                <li class="flex flex-col gap-1">
                    <p class="flex gap-3">
                        <Icon icon="fa6-solid:truck-arrow-right" class="my-auto" /> Pickup Address
                    </p>
                    <p class="ml-10">5006 2, Sofia</p>
                </li>
                <li class="flex flex-col gap-1">
                    <p class="flex gap-3">
                        <Icon icon="fa6-solid:location-crosshairs" class="my-auto" /> Delivery Address
                    </p>
                    <p class="ml-10">5006 2, Sofia</p>
                </li>
                <li class="flex flex-col gap-1">
                    <p class="flex gap-3">
                        <Icon icon="fa6-solid:note-sticky" class="my-auto" /> Delivery Note
                    </p>
                    <p class="ml-10">Please deliver the package to the front door.</p>
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
            <RouterLink class="w-full" to="/">
                <button type="button"
                    class="bg-green-500 text-white text-sm font-bold h-full w-full rounded-r-2xl p-3 border-x-2 border-y-2 border-green-400">
                    Complete Order
                </button>
            </RouterLink>
        </header>
    </div>
</template>

<script setup>
// Import the needed libraries
import { ref, onMounted } from 'vue'
import { useRoute } from 'vue-router'
import { Icon } from '@iconify/vue'
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

// const route = useRoute();
// const quakeId = ref(route.params.id);
const isLoading = ref(true);
const mapRef = ref(null);
const overlayRef = ref(null);
const overlayContent = ref({
    feltScore: {
        description: '',
        value: 0,
    },
    damageScore: {
        description: '',
        value: 0,
    },
});
const isCardVisible = ref(false);
const cardContent = ref({
    place: 'LOREM IPSUM',
    mag: 0,
    time: new Date('2022-01-01T00:00:00'),
    lon: 0,
    lat: 0,
    depth: 0,
});

// console.log(`https://www.seismicportal.eu/fdsnws/event/1/query?limit=10&eventid=${quakeId.value}&format=json`)

// Function to get the color class for the earthquake marker,
// based on its magnitude
function getColorClass(mag) {
    if (mag <= 2.0) {
        return '#bbf7d0';
    } else if (mag <= 3.0) {
        return '#99f6e4';
    } else if (mag <= 4.0) {
        return '#a5f3fc';
    } else if (mag <= 5.0) {
        return '#bfdbfe';
    } else if (mag <= 6.0) {
        return '#c17777';
    } else {
        return '#a73b3b';
    }
}

// Function to get the score color class for the feedback overlays,
// based on its magnitude
function getScoreColorClass(mag) {
    if (mag <= 2.0) {
        return '#00917d';
    } else if (mag <= 3.0) {
        return '#10b981';
    } else if (mag <= 4.0) {
        return '#c17777';
    } else {
        return '#a73b3b';
    }
}

const { data: ordersData } = await useFetch(`https://localhost:7230/api/orders/vendor/3013`);
const userOrders = ref([]);
userOrders.value = ordersData.value;
console.log(ordersData.value);


// The onMounted hook runs when the component is mounted to the website DOM,
// it allows us to access the DOM (HTML) elements and initialize the map
onMounted(() => {
    const map = new Map({
        // Reference the mapRef variable in the template to set the map target
        target: mapRef.value,
        // The layers array holds the map data and markers
        layers: [
            // Create a new map tile layer from the OpenStreetMap source
            new TileLayer({
                source: new OSM()
            }),
            // Create a new vector layer from the GeoJSON source,
            // which holds the earthquake data and marker descriptions
            // The style function sets the marker radius and color
            new VectorLayer({
                source: new VectorSource({
                    url: `https://www.seismicportal.eu/fdsnws/event/1/query?limit=1&eventid=20240427_0000042&format=json`,
                    format: new GeoJSON(),
                }),
                style: function (feature) {
                    const mag = feature.get('mag');
                    const radius = 5 + (mag / 10) * 20;
                    const colorClass = getColorClass(mag);
                    return new Style({
                        image: new CircleStyle({
                            radius: radius,
                            fill: new Fill({
                                color: colorClass,
                            }),
                            stroke: new Stroke({
                                color: 'white',
                                width: 2,
                                opacity: 0.5
                            }),
                        }),
                    });
                },
            }),
        ],
        // The view defines the center and zoom level of the map
        // We also use the current position of the user to center the map,
        // and remove any default controls (zoom, rotate, etc.)
        view: new View({
            center: fromLonLat([0, 0]),
            zoom: 7,
        }),
        controls: []
    });

    // Create a new vector layer for the user feedback markers
    const feedbackMarkerLayer = new VectorLayer({
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

    // Add the offset marker layer to the map
    map.addLayer(feedbackMarkerLayer);

    // Get the vector source of the GeoJSON earthquake layer
    const vectorSource = map.getLayers().item(1).getSource();

    // Wait for the vector source to load the features
    vectorSource.once('change', () => {
        // Get the features from the vector source
        const features = vectorSource.getFeatures();

        // Check if there is at least one feature
        if (features.length > 0) {
            // Get the geometry of the first feature
            const geometry = features[0].getGeometry();
            const coordinate = geometry.getCoordinates();

            // Create three new features with an offset from the earthquake, which serve as placeholder feedback markers
            const offset = 30000; // adjust this value to change the distance of the offset markers
            const feature1 = new Feature({
                geometry: new Point([coordinate[0] + offset, coordinate[1] + offset]),
                feltScore: {
                    description: 'Средно тресене',
                    value: 2,
                },
                damageScore: {
                    description: 'Няма поражение',
                    value: 1,
                },
            });
            const feature2 = new Feature({
                geometry: new Point([coordinate[0] - offset, coordinate[1] + offset]),
                feltScore: {
                    description: 'Силно тресене',
                    value: 3,
                },
                damageScore: {
                    description: 'Малко поражение',
                    value: 2,
                },
            });
            const feature3 = new Feature({
                geometry: new Point([coordinate[0], coordinate[1] - offset]),
                feltScore: {
                    description: 'Леко тресене',
                    value: 1,
                },
                damageScore: {
                    description: 'Няма поражение',
                    value: 1,
                },
            });

            // Add the new features to the feedback marker layer
            feedbackMarkerLayer.getSource().addFeatures([feature1, feature2, feature3]);

            // Get the feature properties
            const { flynn_region, mag, time, depth } = features[0].getProperties();

            // If there is no place, magnitude or time,
            // we don't want to display the overlay
            if (!flynn_region || !mag || !time || !depth)
                return;

            // Format the longitude and latitude coordinates
            const lon = coordinate[0].toFixed(2);
            const lat = coordinate[1].toFixed(2);
            const formattedLon = lon.slice(0, 2) + lon.slice(lon.indexOf('.'), lon.indexOf('.') + 3);
            const formattedLat = lat.slice(0, 2) + lat.slice(lat.indexOf('.'), lat.indexOf('.') + 3);

            cardContent.value.place = flynn_region;
            cardContent.value.mag = mag;
            cardContent.value.time = new Date(time);
            cardContent.value.lon = formattedLon;
            cardContent.value.lat = formattedLat;
            cardContent.value.depth = depth;

            // Center the map on the coordinates of the first feature
            map.getView().setCenter(coordinate);

            // Set the loading state to false
            isLoading.value = false;
        }
    });

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
                const { feltScore, damageScore } = feature.getProperties();

                // If there is no feltScore or damageScore,
                // we don't want to display the overlay
                if (!feltScore || !damageScore)
                    return;

                // Display the overlay and set its content
                overlay.setPosition(feature.getGeometry().getCoordinates());
                overlayContent.value.feltScore = feltScore;
                overlayContent.value.damageScore = damageScore;
            }
        });
    });
});
</script>
