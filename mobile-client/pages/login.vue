<template>
    <div class="min-h-screen bg-red-500 flex justify-center items-center">
        <div class="bg-white rounded-lg p-6 mx-2 max-w-md w-full">
        <h1 class="text-red-800 text-2xl font-bold mb-4">LocalFood | Login</h1>
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
        <div class="mb-6">
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
            <button @click="login()" class="bg-red-800 hover:bg-red-700 text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline">
            Login
            </button>
        </div>
        <div class="text-center mt-4">
            <p>Don't have an account? <NuxtLink to="/register"><span class="underline hover:no-underline cursor-pointer font-semibold">Sign up</span></NuxtLink></p>
        </div>
        </div>
    </div>
</template>

<script setup>
import { storeToRefs } from 'pinia'; // import storeToRefs helper hook from pinia
import { useAuthStore } from '~/store/auth'; // import the auth store we just created

const { authenticateUser } = useAuthStore(); // use authenticateUser action from  auth store
const { authenticated } = storeToRefs(useAuthStore()); // make authenticated state reactive with storeToRefs

const user = ref({
  email: '', 
  password: '',
});
const errorText = ref('');
const router = useRouter();

const validateEmail = () => {
    const emailRegex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;

    if (!emailRegex.test(user.value.email.trim())) {
        return false;
    } else {
        return true;
    }
}

const login = async () => {
  await authenticateUser(user.value); // call authenticateUser and pass the user object

  // redirect to dashboard if user is authenticated
  if (authenticated.value && validateEmail()) {
    router.push('/');
  }
  else{
    errorText.value = 'Invalid username or password';
  }
};
</script>
