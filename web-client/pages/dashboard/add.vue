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
                        <img :src="imagePreview" :class="{ 'opacity-0': !imagePreview }" class="absolute inset-0 w-full h-full cursor-pointer object-cover object-center">
                        <input type="file" @change="onFileChange" class="opacity-0 absolute inset-0 w-full h-full cursor-pointer" accept="image/png, image/gif, image/jpeg">
                    </div>
                    <input type="text" class="text-xl font-bold mb-8" placeholder="Product Title" v-model="product.name">
                    <div class="relative mb-8">
                        <input type="number" class="text-lg font-semibold pl-8 w-28" placeholder="Price" step=".01" v-model="product.price">
                        <span class="absolute top-0 left-0 flex items-center pl-3 text-gray-600 font-semibold text-lg">$</span>
                    </div>
                    <button @click.prevent="addProduct()" class="bg-red-700 text-white py-2 rounded-lg w-32">Submit</button>
                    <p class="text-red-500 mt-2">{{ errorText }}</p> 
                </form>
            </div>
    </section>
</template>

<script setup>
const product = ref({
  name: '',
  price: '', 
  sellerId: '',
  image: null,
});
const imagePreview = ref(null);
const errorText = ref('');
const router = useRouter();
const userId = useCookie('userId');

const onFileChange = (e) => {
  product.value.image = e.target.files[0];
  console.log(product.value.image);
  previewImage(e.target.files[0]);
};

const previewImage = (file) => {
  const reader = new FileReader();
  reader.onload = (e) => {
    imagePreview.value = e.target.result;
  };
  reader.readAsDataURL(file);
};

const addProduct = async () => {
  try {
    const formData = new FormData();
    formData.append('name', product.value.name);
    formData.append('price', product.value.price);
    formData.append('vendorId', userId.value);
    formData.append('image', product.value.image);

    const data = await $fetch('https://localhost:7230/api/products', {
      method: 'post',
      body: formData,
    });
    // console.log(userId.value)
    // const data = await $fetch('https://localhost:7230/api/products', {
    //   method: 'post',
    //   headers: { 'Content-Type': 'application/json' },
    //     body: JSON.stringify({ name: product.value.name, price: product.value.price, vendorId: userId.value }),
    // });

    if (data) {
        // Handle success response 
        router.push('/dashboard');
    } else {
      // Handle error response
      errorText.value = 'Something went wrong!';
    }
  } catch (error) {
    // Handle fetch error
    errorText.value = 'Something went wrong!';
  }
};
</script>