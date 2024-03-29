<template>
    <header class="lg:px-16 px-4 py-6">
        <nav class="flex justify-between relative">
            <NuxtLink to="/">
                <span class="text-red-700 hover:text-red-500 cursor-pointer text-4xl font-bold"
                    style="font-family: calibri;">LocalFood</span>
            </NuxtLink>
            <div class="flex items-center gap-2 cursor-pointer" @click="toggleCart">
                <span class="font-semibold">{{ cartText }}</span>
                <Icon name="material-symbols:shopping-cart" class="text-2xl" />
            </div>
            <div class="absolute z-50 top-16 right-0 bg-white shadow-lg border-2 rounded-lg p-8" :class="{'hidden': !isCartOpen}">
                <div class="flex flex-col gap-4 mb-10 max-h-[70vh] overflow-y-auto">
                    <div v-for="(item, index) in items" :key="index" class="grid grid-cols-12 gap-5 bg-white h-24 rounded-xl max-w-sm text-gray-700">
                        <img class="col-span-3 object-cover object-center w-full h-18 rounded-xl" src="/assets/cheese.jpg">
                        <div class="col-span-9 flex flex-col pt-4 pb-2 px-4">
                            <h1 class="text-lg font-semibold mb-2">{{ item.name }}</h1>
                            <span class="flex justify-end mt-auto">
                                <p class="text-xl font-bold">${{ item.price.toFixed(2) }}</p>
                            </span>
                        </div>
                    </div>
                </div>
                <div class="flex justify-between items-center">
                    <div class="font-bold text-xl w-full">Total: ${{ totalPrice.toFixed(2) }}</div>
                    <NuxtLink to="/checkout">
                        <button class="bg-red-600 text-white px-4 py-2 rounded-lg hover:bg-red-500">Checkout</button>
                    </NuxtLink>
                </div>
            </div>
        </nav>
    </header>
    <slot />
</template>

<script setup>
import { storeToRefs } from 'pinia'; 
import { useCartStore } from '~/store/cart'; 

const isCartOpen = ref(false);
const { items } = storeToRefs(useCartStore()); 

const totalPrice = computed(() => {
    return items.value.reduce((total, item) => total + item.price, 0);
});

const cartText = computed(() => {
    return items.value.length === 0 ? 'Your cart' : `$${items.value.reduce((total, item) => total + item.price, 0.00).toFixed(2)}`;
});

const toggleCart = () => {
    if (items.value.length === 0) 
        return;

    isCartOpen.value = !isCartOpen.value;
}

</script>