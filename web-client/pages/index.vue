<template>
  <div class="relative h-[85vh] w-full overflow-hidden">
    <div class="absolute z-10 text-white h-full lg:w-[55%] w-full flex flex-col justify-center">
      <div class="absolute bg-red-500 h-full lg:w-[80%] w-full left-0 top-0"></div>
      <div class="hidden lg:block absolute bg-red-500 h-full w-96 right-0 top-0 transform skew-x-[-20deg]"></div>
      <div class="relative lg:mr-auto lg:ml-0 mx-auto lg:px-12 px-6">
        <h1 class="text-5xl mb-2 drop-shadow-lg">Shop Local, Support Local</h1>
        <h2 class="text-lg mb-6 drop-shadow-lg">Order food directly from local suppliers</h2>
        <div class="relative">
          <input type="text" name="search" id="search"
            ref="searchInput"
            class="w-full block p-4 pr-10 rounded-md shadow-sm text-black sm:text-sm"
            placeholder="Street Name №, City">
            <span
              @click="getCoordinatesAndRedirect"
              class="absolute inset-y-0 right-0 flex items-center pr-3 text-2xl text-gray-600 hover:text-black cursor-pointer">
              <Icon name="material-symbols:send-rounded" />
            </span>
        </div>
      </div>
    </div>
    <img class="relative -right-24 z-0 ml-auto h-full" src="/assets/hero_banner.jpg">
  </div>
</template>

<script setup>
import { storeToRefs } from 'pinia'; 
import { useUserStore } from '~/store/user'; 

const { personalData } = storeToRefs(useUserStore()); 

definePageMeta({
  layout: 'home'
})

const searchInput = ref(null);

onMounted(() => {
  getUserAddress().then(address => {
    searchInput.value.value = address;
  });
});

const getCoordinatesAndRedirect = async () => {
  const encodedAddress = encodeURIComponent(searchInput.value.value);
  const data = await $fetch(`https://nominatim.openstreetmap.org/search?format=json&q=${encodedAddress}&addressdetails=1&limit=1`);
  console.log(data);
  if (data[0]) {
    const { lat, lon, address } = data[0];

    personalData.value = {};
    personalData.value.lat = lat;
    personalData.value.lon = lon;
    personalData.value.house_number = address.house_number;
    personalData.value.road = address.road;
    personalData.value.city = address.city;

    console.log(personalData.value);

    navigateTo(`/catalog`);
  } else {
    searchInput.value.setCustomValidity('Please enter a valid address');
    searchInput.value.reportValidity();
  }
};

const getUserAddress = async () => {
    let userLocation = {};

    try {
      userLocation = await new Promise((resolve, reject) => {
        navigator.geolocation.getCurrentPosition(resolve, reject);
      });
    }
    catch (error) {
      console.log(error);
      return '';
    }

    const address = await getAddressFromLatLon(userLocation.coords.latitude, userLocation.coords.longitude);

    console.log(address);

    async function getAddressFromLatLon(lat, lon) {
      const data = await $fetch(`https://nominatim.openstreetmap.org/reverse?format=jsonv2&lat=${lat}&lon=${lon}`);
      if (data.address) {
        return `${data.address.road} ${data.address.house_number}, ${data.address.postcode} ${data.address.city}`;
      } else {
        throw new Error('Unable to get address from latitude and longitude');
      }
    }

    return address;
  }
</script>
