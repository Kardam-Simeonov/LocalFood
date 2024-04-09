<template>
    <div class="min-h-screen bg-red-500 flex justify-center items-center">
        <div class="bg-white rounded-lg p-8 max-w-md w-full">
          <h1 class="text-red-800 text-2xl font-bold mb-8">LocalFood | Register</h1>

          <div class="relative h-28 aspect-square mb-6">
              <div class="absolute inset-0 flex justify-center items-center border-dashed border-2 border-red-800">
                  <div class="w-12 aspect-square bg-red-800 rounded-full flex justify-center items-center">
                      <Icon name="bi:plus" class="text-white text-3xl" />
                  </div>
              </div>
              <div id="image-preview" class="opacity-0 absolute inset-0 w-full h-full cursor-pointer bg-cover bg-center"></div>
              <input type="file" class="opacity-0 absolute inset-0 w-full h-full cursor-pointer" accept="image/png, image/gif, image/jpeg">
          </div>
          <div class="mb-4">
            <label class="block text-gray-700 font-bold mb-2" for="name">
              Name
            </label>
            <input class="appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                   id="name"
                   type="text"
                   placeholder="Name"
                   v-model="user.name">
          </div>
          <div class="mb-4">
            <label class="block text-gray-700 font-bold mb-2" for="email">
              Email
            </label>
            <input class="appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                   id="email"
                   type="email"
                   placeholder="Email"
                   v-model="user.email">
          </div>
          <div class="mb-4">
            <label class="block text-gray-700 font-bold mb-2" for="password">
              Password
            </label>
            <input class="appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline"
                   id="password"
                   type="password"
                   placeholder="Password"
                   v-model="user.password">
          </div>
          <p class="text-red-500 text-sm mb-4">{{ errorText }}</p>
          <div class="flex items-center justify-between">
            <button @click="register()" class="bg-red-800 hover:bg-red-700 text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline">
              Register
            </button>
          </div>
        </div>
      </div>
</template>

<script setup>
const user = ref({
  name: '',
  email: '', 
  password: '',
});
const errorText = ref('');
const router = useRouter();

const register = async () => {
  
  console.log(JSON.stringify({ email, password }))
  try {
    const data = await $fetch('https://localhost:7230/api/auth/register', {
      method: 'post',
      headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ name: user.value.name, email: user.value.email, password: user.value.password }), // Ensure body is stringified
    });

    if (data) {
      if (data == 'User already exists.') {
        errorText.value = 'User already exists.';
      } else {
        router.push('/dashboard');
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