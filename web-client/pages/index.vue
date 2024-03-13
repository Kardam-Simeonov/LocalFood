<template>
    <header class="md:px-16 px-4 py-6">
      <nav class="flex justify-between">
        <span class="text-red-700 hover:text-red-500 cursor-pointer text-4xl font-bold mr-8" style="font-family: calibri;">Food2U</span>
        <div class="flex items-center gap-2 cursor-pointer">
          <span class="font-semibold">Login as a Supplier</span>
          <iconify-icon icon="material-symbols:login-rounded" class="text-2xl"></iconify-icon>
        </div>
      </nav>
    </header>
    <main>
      <div class="relative h-[85vh]">
        <div class="absolute z-10 text-white h-full lg:w-[55%] w-full flex flex-col justify-center">
          <div class="absolute bg-red-500 h-full lg:w-[80%] w-full left-0 top-0"></div>
          <div class="hidden lg:block absolute bg-red-500 h-full w-96 right-0 top-0 transform skew-x-[-20deg]"></div>
          <div class="relative lg:mr-auto lg:ml-0 mx-auto lg:px-12 px-6">
            <h1 class="text-5xl mb-2 drop-shadow-lg">Shop Local, Support Local</h1>
            <h2 class="text-lg mb-6 drop-shadow-lg">Order food directly from local suppliers</h2>
            <div class="relative">
              <input type="text" name="search" id="search" class="w-full block p-4 pr-10 rounded-md shadow-sm text-black sm:text-sm" placeholder="Enter your address...">
              <span class="absolute inset-y-0 right-0 flex items-center pr-3 text-2xl text-gray-600 hover:text-black cursor-pointer">
                <iconify-icon icon="material-symbols:send-rounded"></iconify-icon>
              </span>
            </div>
          </div>
        </div>
        <img class="relative -right-24 z-0 ml-auto h-full" src="../src/assets/hero_banner.jpg">
      </div>
    </main>
</template>
