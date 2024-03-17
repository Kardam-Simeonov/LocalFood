<template>
    <header class="lg:px-16 px-4 py-6">
        <nav class="flex justify-between">
            <span class="text-red-700 hover:text-red-500 cursor-pointer text-4xl font-bold"
                style="font-family: calibri;">Food2U | Vendors</span>
            <div class="flex items-center gap-3 cursor-pointer">
                <span class="font-semibold text-xl">Logout</span>
                <Icon name="material-symbols:login-rounded" class="text-3xl" />
            </div>
        </nav>
    </header>
    <main>
        <section class="grid grid-cols-12 xl:h-[90vh]">
            <div id="map" class="xl:col-span-8 col-span-full xl:h-full h-[50vh]"></div>
            <aside
                class="xl:col-span-4 col-span-full bg-red-500 xl:h-full h-[70vh] overflow-y-auto px-8 py-12 flex flex-col">
                <h1 class="text-4xl text-white font-semibold mb-12">Your orders</h1>
                <div
                    class="border-dashed border-4 border-red-800 rounded-lg p-4 flex flex-col justify-center items-center min-h-[15rem]">
                    <p class="text-white text-center">You have no orders</p>
                </div>
            </aside>
        </section>
        <section class="py-12 text-white">
            <h1 class="text-4xl font-bold text-center mb-16 text-red-700">Your Products</h1>
            <div class="grid lg:grid-cols-3 md:grid-cols-2 grid-cols-1 gap-4 max-w-5xl px-4 mx-auto">
                <div class="border-dashed border-4 border-red-500 hover:border-red-300 text-red-500 hover:text-red-300 cursor-pointer rounded-lg p-4 flex flex-col justify-center items-center min-h-[23rem]">
                    <Icon name="bi:plus-circle" class="text-6xl" />
                </div>
            </div>
        </section>
    </main>
</template>