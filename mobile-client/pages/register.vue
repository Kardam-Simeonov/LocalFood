<template>
  <div class="min-h-screen bg-red-500 flex justify-center items-center">
    <div class="bg-white rounded-lg p-6 mx-2 max-w-md w-full">
      <h1 class="text-red-800 text-2xl font-bold mb-8">LocalFood | Register</h1>
      <div class="relative h-28 aspect-square mb-6">
        <div class="absolute inset-0 flex justify-center items-center border-dashed border-2 border-red-800">
          <div class="w-12 aspect-square bg-red-800 rounded-full flex justify-center items-center">
            <Icon name="bi:plus" class="text-white text-3xl" />
          </div>
        </div>
        <img :src="imagePreview" :class="{ 'opacity-0': !imagePreview }"
          class="absolute inset-0 w-full h-full cursor-pointer object-cover object-center">
        <input @change="onFileChange" type="file" class="opacity-0 absolute inset-0 w-full h-full cursor-pointer"
          accept="image/png, image/gif, image/jpeg">
      </div>
      <div class="mb-4">
        <label class="block text-gray-700 font-bold mb-2" for="name">
          Name
        </label>
        <input
          class="appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
          id="name" type="text" placeholder="Name" v-model="user.name">
      </div>
      <div class="mb-4">
        <label class="block text-gray-700 font-bold mb-2" for="address">
          Phone Number
        </label>
        <input
          class="appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
          id="phoneNumber" type="text" placeholder="PhoneNumber" v-model="user.phoneNumber">
      </div>
      <div class="mb-4">
        <label class="block text-gray-700 font-bold mb-2" for="email">
          Email
        </label>
        <input
          class="appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
          id="email" type="email" placeholder="Email" v-model="user.email">
      </div>
      <div class="mb-4">
        <label class="block text-gray-700 font-bold mb-2" for="password">
          Password
        </label>
        <input
          class="appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
          id="password" type="password" placeholder="Password" v-model="user.password">
      </div>
      <p class="text-red-500 text-sm mb-4">{{ errorText }}</p>
      <div class="flex items-center justify-between">
        <button @click="register()"
          class="bg-red-800 hover:bg-red-700 text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline">
          Register
        </button>
      </div>
      <div class="text-center mt-4">
          <p>Already have an account? <NuxtLink to="/login"><span class="underline hover:no-underline cursor-pointer font-semibold">Login</span></NuxtLink></p>
      </div>
    </div>
  </div>
</template>

<script setup>
const user = ref({
  name: '',
  email: '',
  phoneNumber: '',
  password: '',
  image: null
});
const imagePreview = ref(null);
const errorText = ref('');
const router = useRouter();

const onFileChange = (e) => {
  user.value.image = e.target.files[0];
  console.log(user.value.image);
  previewImage(e.target.files[0]);
};

const previewImage = (file) => {
  const reader = new FileReader();
  reader.onload = (e) => {
    imagePreview.value = e.target.result;
  };
  reader.readAsDataURL(file);
};

const register = async () => {
  const formData = new FormData();
  formData.append('name', user.value.name);
  formData.append('email', user.value.email);
  formData.append('password', user.value.password);
  formData.append('phoneNumber', user.value.phoneNumber);
  formData.append('image', user.value.image);

  try {
    const data = await $fetch('https://localhost:7230/api/auth/driver/register', {
      method: 'post',
      body: formData
    });

    if (data) {
      if (data == 'User already exists.') {
        errorText.value = 'User already exists.';
      } else {
        router.push('/');
      }
    } else {
      // Handle error response
      errorText.value = 'User already exists.';
    }
  } catch (error) {
    // Handle fetch error
    errorText.value = 'User already exists.';
  }
};
</script>