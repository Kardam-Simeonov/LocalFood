<template>
    <header class="lg:px-16 px-4 py-6">
        <nav class="flex justify-between">
            <span class="text-red-700 hover:text-red-500 cursor-pointer text-4xl font-bold"
                style="font-family: calibri;">LocalFood | Vendors</span>
            <div class="flex items-center gap-3 cursor-pointer">
                <span class="font-semibold text-xl">Logout</span>
                <Icon name="material-symbols:login-rounded" class="text-3xl" />
            </div>
        </nav>
    </header>
    <slot />
</template>