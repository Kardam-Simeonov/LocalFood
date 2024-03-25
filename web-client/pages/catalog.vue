<template>
    <main>
        <section class="grid grid-cols-12">
            <aside
                class="lg:block hidden col-span-4 bg-[url('/assets/catalog_banner.jpg')] h-screen bg-cover brightness-75 sticky top-0">
            </aside>
            <div class="lg:col-span-8 col-span-full bg-red-500 h-screen px-8 py-12 flex flex-col">
                <h1 class="text-3xl font-bold mb-12 text-white">Showing {{ catalogItems.length }} offer in your area</h1>
                <div class="flex flex-col gap-4">
                    <div v-for="(item, index) in catalogItems" :key="index" @click="addToCart(catalogItems[index])" class="grid grid-cols-12 bg-white h-40 rounded-xl shadow-lg max-w-4xl text-gray-700 hover:bg-gray-100 cursor-pointer">
                        <img class="col-span-4 object-cover object-center h-40 w-full rounded-l-xl"
                            :src="item.image">
                        <div class="col-span-8 flex flex-col pt-4 pb-2 px-4">
                            <h1 class="text-xl font-semibold mb-2">{{ item.name }}</h1>
                            <span class="flex gap-2">
                                <img class="w-8 aspect-square rounded-full object-cover object-center"
                                    :src="item.seller.image">
                                <p class="font-semibold">{{ item.seller.name }}</p>
                            </span>
                            <span class="flex justify-between mt-auto">
                                <p class="text-xl font-bold">${{ item.price.toFixed(2) }}</p>
                                <div class="flex items-center gap-2">
                                    <Icon name="fa6-solid:location-arrow" class="text-gray-600 text-lg" />
                                    <p>{{ item.distance }}km away</p>
                                </div>
                            </span>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </main>
</template>

<script setup>
import { storeToRefs } from 'pinia'; 
import { useCartStore } from '~/store/cart'; 

const { addToCart, removeFromCart, clearCart } = useCartStore(); 
const { items } = storeToRefs(useCartStore()); 

const catalogItems = ref([
  {
    name: 'Homemade Feta Cheese',
    price: 5.00,
    distance: 1.5,
    image: '/assets/cheese.jpg',
    seller: {
      name: 'Ivan',
      image: '/assets/profile.jpg'
    }
  },
]);

definePageMeta({
  layout: 'cart'
})

</script>