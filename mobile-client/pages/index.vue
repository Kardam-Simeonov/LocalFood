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
      <div id="map" ref="mapRef" class="w-full z-0 absolute inset-0"></div>
      <div id="overlay" ref="overlayRef" class="relative" :class="{ 'invisible': isLoading }">
        <RouterLink :to="`/earthquake/${overlayContent.id}`">
          <div
            class="relative z-10 bg-white text-quakeGreen-dark text-xs rounded-lg p-2 w-[13rem] grid grid-cols-3 gap-x-1 gap-y-2">
            <div class="col-span-1 flex items-center justify-center">
              <p class="flex items-center justify-center aspect-square rounded-full text-center text-sm p-3 font-bold text-cyan-700 border-[1px] border-neutral-200 shadow"
                :style="{ 'background-color': getColorClass(overlayContent.mag) }">
                {{ overlayContent.mag.toFixed(1) }}
              </p>
            </div>
            <div class="col-span-2">
              <h1 class="text-bold text-quakeGreen mb-1">{{ overlayContent.place }}</h1>
              <p>{{ overlayContent.date }}</p>
            </div>
            <div class="col-span-full border-t-2 border-neutral-100 pt-1">
              <p class="text-center font-medium text-quakeGreen">Научете повече</p>
            </div>
          </div>
        </RouterLink>
        <div class="w-0 h-0 absolute z-0 -bottom-2 left-0 right-0 mx-auto
                                    border-l-[40px] border-l-transparent
                                    border-t-[35px] border-t-white
                                    border-r-[40px] border-r-transparent">
        </div>
      </div>
      <div class="absolute top-24 p-2 m-1 bg-white bg-opacity-40 rounded-lg text-teal-800 text-sm">
        Tiles &copy; <a href="https://www.openstreetmap.org/">OpenStreetMap</a> contributors<br>
        Data &copy; <a href="https://earthquake.usgs.gov/">SeismicPortal</a>
      </div>
      <section
        class="absolute z-50 transition-transform duration-500 left-0 right-0 flex flex-col gap-10 h-[64rem] w-full py-7 px-3 bg-red-500 bg-opacity-95 shadow-lg border-t-4 border-red-400 rounded-[2rem]"
        :style="{ transform: !isTableVisible ? 'translateY(calc(100vh - 11rem))' : 'none' }">
        <div class="absolute z-10 top-0 w-full h-20" @click="isTableVisible = !isTableVisible"></div>
        <div
          class="absolute flex justify-center -top-1 left-0 right-0 mx-auto rounded-full w-24 text-4xl font-bold text-red-700">
          <Icon class="mb-1" icon="fa6-solid:grip-lines"/>
        </div>
        <h1 class="text-white font-bold text-xl text-center drop-shadow-md">Deliveries in this area</h1>
        <div class="overflow-y-auto" style="height: calc(100vh - 13.5rem); width: 100%">
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
              <button @click="deleteOrder(order.id)"
                  class="bg-green-500 text-white px-4 py-2 rounded-lg hover:bg-green-600 focus:outline-none focus:ring-2 focus:ring-green-500 focus:ring-opacity-50 mt-4">View more details</button>
          </div>
          <!-- <div class="py-4 mb-4 bg-white rounded-xl cursor-pointer" v-for="(quake, index) in quakeData" :key="index">
            <RouterLink :to="`/earthquake/${quake.id}`">
              <div class="relative flex p-2 gap-5 ">
                <p class="flex items-center justify-center aspect-square rounded-full text-center text-2xl p-1 w-20 h-20 font-bold text-cyan-700 shadow"
                  :style="{ 'background-color': getColorClass(quake.mag) }">
                  {{ quake.mag }}
                </p>
                <div class="flex flex-col justify-center">
                  <h1 class="font-semibold truncate text-quakeGreen-dark mb-1 ml-2 text-base whitespace-normal break-normal">
                    {{
                      quake.place }}</h1>
                  <p class="ml-2 truncate text-quakeGreen">{{ new Date(quake.time).toLocaleDateString() }}</p>
                </div>
              </div>
            </RouterLink>
          </div> -->
        </div>
        <!-- <tr class="h-12 w-full border-y-[1px]" v-for="(quake, index) in quakeData" :key="index">
            <td>{{ index + 1 }}</td>
            <td>{{ quake.time }}</td>
            <td>{{ quake.mag }}</td>
            <td>{{ quake.lat }}</td>
            <td>{{ quake.lon }}</td>
            <td>{{ quake.depth }}</td>
          </tr> -->
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
const quakeData = ref([
]);

// Variables for the map and overlay instance,
// along with the overlay content object, which holds:
// the place name, magnitude and time of the earthquake
const mapRef = ref(null);
const overlayRef = ref(null);
const overlayContent = ref({
  id: '',
  place: '',
  mag: 0,
  time: new Date().toLocaleString()
});

// Function to get the color class for the earthquake marker,
// based on its magnitude. 
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

const { data: ordersData } = await useFetch(`https://localhost:7230/api/orders/vendor/3013`);
const userOrders = ref([]);
userOrders.value = ordersData.value;
console.log(ordersData.value);

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
        // Create a new vector layer from the GeoJSON source,
        // which holds the earthquake data and marker descriptions
        // The style function sets the marker radius and color
        new VectorLayer({
          source: new VectorSource({
            url: 'https://www.seismicportal.eu/fdsnws/event/1/query?limit=300&minmag=2.0&minlat=36.351437&maxlat=45.919546&minlon=18.819906&maxlon=33.260681&format=json',
            format: new GeoJSON(),
          }),
          style: function (feature) {
            const mag = feature.get('mag');
            const radius = 5 + (mag / 10) * 15;
            const colorClass = getColorClass(mag);
            return new Style({
              image: new CircleStyle({
                radius: radius,
                fill: new Fill({
                  color: colorClass,
                }),
                stroke: new Stroke({
                  color: 'white',
                  width: 1,
                  opacity: 0.1
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
        center: fromLonLat([position.coords.longitude, position.coords.latitude]),
        zoom: 5,
      }),
      controls: []
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
          console.log(feature);
          // Center the map view on the clicked feature
          const view = map.getView();
          view.animate({
            center: feature.getGeometry().getCoordinates(),
            duration: 1000
          });

          // Get the feature properties
          const { flynn_region, mag, time, unid } = feature.getProperties();

          // If there is no place, magnitude or time,
          // we don't want to display the overlay
          if (!flynn_region || !mag || !time || !unid)
            return;

          // Display the overlay and set its content
          overlay.setPosition(feature.getGeometry().getCoordinates());
          overlayContent.value.place = flynn_region;
          overlayContent.value.mag = mag;
          overlayContent.value.date = new Date(time).toLocaleDateString();
          overlayContent.value.id = unid;
        }
      });
    });

    // Get the vector source of the GeoJSON earthquake layer
    const vectorSource = map.getLayers().item(1).getSource();

    // Wait for the vector source to load the features
    vectorSource.once('change', () => {
      // Get the features from the vector source
      const features = vectorSource.getFeatures();

      // Check if there is at least one feature
      if (features.length > 0) {
        // Loop through all features and map the following properties to the quakeData array, as a new object: lon, lat, depth, mag, time.
        quakeData.value = features.map(feature => {
          let { lon, lat, depth, mag, time, flynn_region, unid } = feature.getProperties();
          lon = `${lon}°N`;
          lat = `${lat}°E`;
          depth = `${depth.toFixed(1)} km`;
          mag = mag.toFixed(1);

          const formattedTime = new Date(time).toLocaleTimeString('en-US', { hour: '2-digit', minute: '2-digit', hour12: false });
          const formattedDate = new Date(time).toLocaleDateString();
          time = `${formattedTime} ${formattedDate}`;

          return {
            lon,
            lat,
            depth,
            mag,
            time,
            place: flynn_region,
            id: unid
          }
        }).sort((a, b) => new Date(b.time) - new Date(a.time));
      }
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
  });
});
</script>
