<template>
    <header class="md:px-16 px-4 py-6">
      <nav class="flex justify-between">
        <span class="text-red-700 hover:text-red-500 cursor-pointer text-4xl font-bold mr-8" style="font-family: calibri;">LocalFood</span>
          <NuxtLink to="/login" class="flex items-center gap-2 cursor-pointer">
            <span class="font-semibold">Login as a Supplier</span>
            <Icon name="material-symbols:login-rounded" class="text-2xl" />
          </NuxtLink>
      </nav>
    </header>
    <slot />
</template>