<template>
  <div class="relative overflow-hidden h-screen">
    <nav class="relative z-40 h-24 pt-6 px-8 bg-white bg-opacity-90 text-quakeGreen flex justify-between">
      <div class="flex text-red-700">
        <div class="w-36 text-xl">
          <h1 class="font-bold">LocalFood</h1>
          <p>Driver</p>
        </div>
      </div>
      <div class="flex items-center gap-2 cursor-pointer bg-gray-100 hover:bg-gray-200 rounded-full px-6 py-1 mb-4">
        <span class="font-semibold text-base">Hello, User</span>
        <img class="w-8 aspect-square object-cover rounded-full" :src="imagePreview">
      </div>

    </nav>
    <main class="flex h-full min-h-screen bg-quakeGreen-background">
      <div id="map" ref="mapRef" class="w-full z-0 absolute inset-0 h-screen"></div>
      <div v-if="showOverlay" class="absolute left-0 right-0 bottom-24 bg-white rounded-lg max-h-64 w-[90%] mx-auto overflow-y-auto" :class="{ 'invisible': isLoading }">
        <h1 class="font-bold text-lg mb-2 mt-4 ml-4">Vendor's Orders</h1>
        <div v-for="(order, index) in overlayContent.orders" :key="index" class="p-6">
            <div class="font-bold text-base mb-4">Order #{{ order.id }}</div>
            <div v-for="(orderProduct, index) in order.orderProducts" :key="index" class="flex items-center mb-1">
              <img :src="'data:image/jpeg;base64,' + orderProduct.product.image"
                class="w-8 h-8 object-cover rounded-lg mr-4">
              <div>
                <div class="font-bold text-sm">{{ orderProduct.product.name }}</div>
                <div class="text-gray-600 text-sm">${{ orderProduct.product.price.toFixed(2) }}</div>
              </div>
            </div>
            <p class="font-semibold text-sm mt-2">Deliver to: {{ order.address }}</p>
            <NuxtLink :to="`delivery/order-${order.id}/vendor-${order.vendorId}`">
              <button class="text-white px-4 py-2 rounded-lg mt-4 text-sm font-semibold text-center w-full bg-green-500">Take order</button>
            </NuxtLink>
          </div>
      </div>
      <div class="absolute top-24 p-2 m-1 bg-white bg-opacity-40 rounded-lg text-teal-800 text-sm">
        Tiles &copy; <a href="https://www.openstreetmap.org/">OpenStreetMap</a> contributors
      </div>
      <section
        class="absolute z-50 transition-transform duration-500 left-0 right-0 flex flex-col gap-10 h-[64rem] w-full py-7 px-3 bg-red-500 bg-opacity-95 shadow-lg border-t-4 border-red-400 rounded-[2rem]"
        :style="{ transform: !isTableVisible ? 'translateY(calc(100vh - 11rem))' : 'none' }">
        <div class="absolute z-10 top-0 w-full h-20" @click="isTableVisible = !isTableVisible"></div>
        <div
          class="absolute flex justify-center -top-1 left-0 right-0 mx-auto rounded-full w-24 text-4xl font-bold text-red-700">
          <Icon class="mb-1" icon="fa6-solid:grip-lines" />
        </div>
        <h1 class="text-white font-bold text-xl text-center drop-shadow-md">Deliveries in this area</h1>
        <div class="overflow-y-auto" style="height: calc(100vh - 13.5rem); width: 100%">
          <div v-for="(order, index) in userOrders" :key="index" class="bg-white shadow-lg rounded-lg p-6 mb-6">
            <div class="font-bold text-xl mb-4">Order #{{ order.id }}</div>
            <div v-for="(orderProduct, index) in order.orderProducts" :key="index" class="flex items-center mb-4">
              <img :src="'data:image/jpeg;base64,' + orderProduct.product.image"
                class="w-16 h-16 object-cover rounded-lg mr-4">
              <div>
                <div class="font-bold">{{ orderProduct.product.name }}</div>
                <div class="text-gray-600">${{ orderProduct.product.price.toFixed(2) }}</div>
              </div>
            </div>
            <p class="font-semibold">Deliver to: {{ order.address }}</p>
            <NuxtLink :to="`delivery/order-${order.id}/vendor-${order.vendorId}`">
              <button
                class="bg-green-500 text-white px-4 py-2 rounded-lg hover:bg-green-600 focus:outline-none focus:ring-2 focus:ring-green-500 focus:ring-opacity-50 mt-4">
                Take order</button>
            </NuxtLink>
          </div>
        </div>
      </section>
    </main>
  </div>
</template>

<script setup>
// Import the needed libraries
import { ref, onMounted } from 'vue'
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
import Point from 'ol/geom/Point';
import Feature from 'ol/Feature';
import { Circle as CircleStyle, Fill, Stroke, Style } from 'ol/style';
import { fromLonLat } from 'ol/proj';

// Variables for the table, which displays earthquake data
// and the isLoading variable, which hides the map until it loads
const isLoading = ref(true);
const isTableVisible = ref(false);

// Variables for the map and overlay instance,
// along with the overlay content object, which holds:
// the place name, magnitude and time of the earthquake
const mapRef = ref(null);
const overlayContent = ref({
  orders: []
});
const showOverlay = ref(false);

const { data: ordersData } = await useFetch(`https://localhost:7230/api/orders`);
const userOrders = ref([]);

// Function to split orders by vendor
function splitProductsByVendor(orders) {
  const splitOrders = [];
  orders.forEach(order => {
    const orderProductsByVendor = {};
    order.orderProducts.forEach(orderProduct => {
      const vendorId = orderProduct.product.vendorId;
      if (!orderProductsByVendor[vendorId]) {
        // If the vendor ID doesn't exist in the order, create a new array for it
        orderProductsByVendor[vendorId] = {
          id: order.id,
          address: order.address,
          latitude: order.latitude,
          longitude: order.longitude,
          vendorId: vendorId,
          orderProducts: []
        };
      }
      // Add product to the respective vendor's array
      orderProductsByVendor[vendorId].orderProducts.push(orderProduct);
    });
    // Convert the object into an array and push it to the splitOrders array
    Object.values(orderProductsByVendor).forEach(order => splitOrders.push(order));
  });
  return splitOrders;
}

userOrders.value = splitProductsByVendor(ordersData.value);

userOrders.value = userOrders.value.filter(order => 
    order.orderProducts.length > 0 && order.orderProducts[0].readyForPickup
);

// Create a ref to store vendor locations
const vendorLocations = ref({});

for (const order of userOrders.value) {
  const vendorId = order.vendorId;
  if (!vendorLocations.value[vendorId]) {
    // Fetch vendor location if it's not already stored
    const { data: response } = await useFetch(`https://localhost:7230/api/auth/vendor/${vendorId}`);
    vendorLocations.value[vendorId] = [response.value.longitude, response.value.latitude];
  }
}

// The onMounted hook runs when the component is mounted to the website DOM,
// it allows us to access the DOM (HTML) elements and initialize the map
onMounted(() => {
  // Using the Capacitor Geolocation API to get the current position of the user
  const currentPosition = async () => {
    return await Geolocation.getCurrentPosition();
  }

  // When the current position is retrieved, we can initialize the map
  currentPosition().then(position => {
    // Stop the loading and initialize the map
    isLoading.value = false;
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

    // When a marker is clicked, get its properties and display them in the overlay
    map.on('singleclick', (event) => {
      // Hide the overlay by default,
      // in case the user clicks on the map, instead of a marker
      showOverlay.value = false;

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
          const vendorOrders = feature.get('vendorOrders');

          console.log(vendorOrders);

          // If there is no place, magnitude or time,
          // we don't want to display the overlay
          if (!vendorOrders)
            return;

          // Display the overlay and set its content
          overlayContent.value.orders = vendorOrders;
          showOverlay.value = true;
        }
      });
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

    // Fetch vendor locations and add markers to the map
    const vendorMarkerSource = new VectorSource();

    for (const vendorId in vendorLocations.value) {
      // Create a new feature for the vendor location
      const vendorMarker = new Feature({
        geometry: new Point(fromLonLat(vendorLocations.value[vendorId])),
      });

      // Get all orders for the current vendor
      const vendorOrders = userOrders.value.filter(order => order.vendorId == vendorId);

      // Add the vendorOrders property to the feature
      vendorMarker.set('vendorOrders', vendorOrders);

      vendorMarkerSource.addFeature(vendorMarker);
    }
    // Create a new layer for the vendor markers and add it to the map
    const vendorMarkerLayer = new VectorLayer({
      source: vendorMarkerSource,
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
    map.addLayer(vendorMarkerLayer);
  });
});
</script>
