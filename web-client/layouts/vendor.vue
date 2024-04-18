<template>
    <header class="lg:px-16 px-4 py-6">
        <nav class="flex justify-between">
            <span class="text-red-700 hover:text-red-500 cursor-pointer text-4xl font-bold"
                style="font-family: calibri;">LocalFood | Vendors</span>
            <div class="flex gap-6">
                <NuxtLink to="/account">
                    <div class="flex items-center gap-2 cursor-pointer bg-gray-100 hover:bg-gray-200 rounded-full px-6 py-1">
                        <span @click="logout()" class="font-semibold text-base">Hello, Sali Yashar</span>
                        <img class="w-8 aspect-square object-cover rounded-full" src="/assets/profile.jpg">
                    </div>
                </NuxtLink>
                <div class="flex items-center gap-3 cursor-pointer">
                    <span @click="logout()" class="font-semibold text-xl">Logout</span>
                    <Icon name="material-symbols:login-rounded" class="text-3xl" />
                </div>
            </div>
        </nav>
    </header>
    <slot />
</template>

<script setup>
import { useAuthStore } from '~/store/auth'; // import the auth store we just created

const { logUserOut } = useAuthStore(); // use authenticateUser action from  auth store

const router = useRouter();

const logout = () => {
  logUserOut();
  router.push('/');
};
</script>