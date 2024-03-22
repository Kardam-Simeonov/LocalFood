<template>
  <div class="relative h-[85vh] w-full overflow-hidden">
    <div class="absolute z-10 text-white h-full lg:w-[55%] w-full flex flex-col justify-center">
      <div class="absolute bg-red-500 h-full lg:w-[80%] w-full left-0 top-0"></div>
      <div class="hidden lg:block absolute bg-red-500 h-full w-96 right-0 top-0 transform skew-x-[-20deg]"></div>
      <div class="relative lg:mr-auto lg:ml-0 mx-auto lg:px-12 px-6">
        <h1 class="text-5xl mb-2 drop-shadow-lg">Shop Local, Support Local</h1>
        <h2 class="text-lg mb-6 drop-shadow-lg">Order food directly from local suppliers</h2>
        <div class="relative">
          <input type="text" name="search" id="search" class="w-full block p-4 pr-10 rounded-md shadow-sm text-black sm:text-sm" placeholder="Enter your address...">
          <NuxtLink to="/catalog">
            <span class="absolute inset-y-0 right-0 flex items-center pr-3 text-2xl text-gray-600 hover:text-black cursor-pointer">
              <Icon name="material-symbols:send-rounded" />
            </span>
          </NuxtLink>
        </div>
      </div>
    </div>
    <img class="relative -right-24 z-0 ml-auto h-full" src="/assets/hero_banner.jpg">
  </div>
</template>

<script setup>
definePageMeta({
  layout: 'home'
})
</script>
