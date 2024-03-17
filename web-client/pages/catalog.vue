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
                    <img class="col-span-4 object-cover object-center w-full h-24 rounded-xl" src="@/assets/cheese.jpg">
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
    <main>
        <section class="grid grid-cols-12">
            <aside
                class="lg:block hidden col-span-4 bg-[url('@/assets/catalog_banner.jpg')] h-screen bg-cover brightness-75 sticky top-0">
            </aside>
            <div class="lg:col-span-8 col-span-full bg-red-500 h-screen px-8 py-12 flex flex-col">
                <h1 class="text-3xl font-bold mb-12 text-white">Showing 1 offer in your area</h1>
                <div class="flex flex-col gap-4">
                    <div class="grid grid-cols-12 bg-white h-40 rounded-xl shadow-lg max-w-4xl text-gray-700">
                        <img class="col-span-4 object-cover object-center h-40 w-full rounded-l-xl"
                            src="@/assets/cheese.jpg">
                        <div class="col-span-8 flex flex-col pt-4 pb-2 px-4">
                            <h1 class="text-xl font-semibold mb-2">Homemade Feta Cheese</h1>
                            <span class="flex gap-2">
                                <img class="w-8 aspect-square rounded-full object-cover object-center"
                                    src="@/assets/profile.jpg">
                                <p class="font-semibold">Ivan</p>
                            </span>
                            <span class="flex justify-between mt-auto">
                                <p class="text-xl font-bold">£5.00</p>
                                <div class="flex items-center gap-2">
                                    <Icon name="fa6-solid:location-arrow" class="text-gray-600 text-lg" />
                                    <p>1.5 miles away</p>
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
const isCartOpen = ref(false);

</script>