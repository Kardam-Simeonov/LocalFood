<template>
    <section class="bg-red-500 min-h-screen flex justify-center items-center">
            <div class="bg-white rounded-lg p-4 flex flex-col justify-between w-96">
                <form>
                    <div class="relative w-full h-56 mb-6">
                        <div class="absolute inset-0 flex justify-center items-center border-dashed border-2 border-red-800">
                            <div class="w-16 h-16 bg-red-800 rounded-full flex justify-center items-center">
                                <Icon name="bi:plus" class="text-white text-3xl" />
                            </div>
                        </div>
                        <div id="image-preview" class="opacity-0 absolute inset-0 w-full h-full cursor-pointer bg-cover bg-center"></div>
                        <input type="file" class="opacity-0 absolute inset-0 w-full h-full cursor-pointer" accept="image/png, image/gif, image/jpeg">
                    </div>
                    <input type="text" class="text-xl font-bold mb-8" placeholder="Product Title">
                    <div class="relative mb-8">
                        <input type="number" class="text-lg font-semibold pl-8 w-28" placeholder="Price" step=".01">
                        <span class="absolute top-0 left-0 flex items-center pl-3 text-gray-600 font-semibold text-lg">$</span>
                    </div>
                    <button type="submit" class="bg-red-700 text-white py-2 rounded-lg w-32">Submit</button>
                    <p class="text-red-500 mt-2">error text field</p> 
                </form>
            </div>
    </section>
</template>