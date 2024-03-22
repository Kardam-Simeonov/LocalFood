<template>
    <header class="lg:px-16 px-4 py-6">
        <nav class="flex justify-between relative">
            <NuxtLink to="/">
                <span class="text-red-700 hover:text-red-500 cursor-pointer text-4xl font-bold"
                    style="font-family: calibri;">LocalFood</span>
            </NuxtLink>
            <div class="flex items-center gap-2 p-4 cursor-pointer" @click="isCartOpen = !isCartOpen">
                <span class="font-semibold">Your cart</span>
                <Icon name="material-symbols:shopping-cart" class="text-2xl" />
            </div>
            <div class="absolute z-50 top-16 right-0 bg-white shadow-lg border-2 rounded-lg p-8" :class="{'hidden': !isCartOpen}">
                <div class="flex flex-col gap-2 mb-10">
                <div class="grid grid-cols-12 gap-5 bg-white h-24 rounded-xl max-w-sm text-gray-700">
                    <img class="col-span-4 object-cover object-center w-full h-24 rounded-xl" src="/assets/cheese.jpg">
                    <div class="col-span-8 flex flex-col pt-4 pb-2 px-4">
                        <h1 class="text-lg font-semibold mb-2">Homemade Feta Cheese</h1>
                        <span class="flex justify-end mt-auto">
                            <p class="text-xl font-bold">$3.50</p>
                        </span>
                    </div>
                </div>
                </div>
                <div class="flex justify-between items-center">
                    <div class="font-bold text-xl w-full">Total: $3.50</div>
                    <button class="bg-red-600 text-white px-4 py-2 rounded-lg hover:bg-red-500">Checkout</button>
                </div>
            </div>
        </nav>
    </header>
    <slot />
</template>

<script setup>
const isCartOpen = ref(false);
</script>