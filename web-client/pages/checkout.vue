<template>
    <header class="lg:px-16 px-4 py-6">
        <nav class="flex justify-between relative">
            <NuxtLink to="/">
                <span class="text-red-700 hover:text-red-500 cursor-pointer text-4xl font-bold"
                    style="font-family: calibri;">LocalFood</span>
            </NuxtLink>
            <div class="flex items-center gap-2 cursor-pointer">
                <span class="font-semibold">Checkout</span>
                <Icon name="material-symbols:shopping-cart-checkout-rounded" class="text-2xl" />
            </div>
        </nav>
    </header>
    <main>
        <section class="grid grid-cols-12">
            <div class="col-span-full bg-red-500 h-fit lg:px-16 px-4 py-12 flex flex-col">
                <h1 class="text-4xl font-bold mb-12 text-white">Checkout</h1>
                <div class="grid grid-cols-12">
                    <div class="lg:col-span-8 col-span-full">
                        <div class="grid grid-cols-12 bg-white rounded-xl shadow-lg max-w-4xl text-gray-700 p-8 h-fit">
                            <h1 class="text-2xl font-bold mb-4 col-span-full">Delivery Address</h1>
                            <div class="col-span-full grid grid-cols-2 gap-4 mb-4">
                                <div class="flex flex-col gap-2">
                                    <label class="font-medium text-gray-700">Street<span
                                            class="text-red-500">*</span></label>
                                    <input v-model="deliveryAddress.street.text" @blur="validateStreet" type="text"
                                        class="appearance-none border border-gray-300 rounded-md py-2 px-4 leading-tight focus:outline-none focus:border-blue-500">
                                    <p class="text-red-500 text-sm">{{ deliveryAddress.street.errorText }}</p>
                                </div>
                                <div class="flex flex-col gap-2">
                                    <label class="font-medium text-gray-700">Number<span
                                            class="text-red-500">*</span></label>
                                    <input v-model="deliveryAddress.number.text" @blur="validateNumber" type="text"
                                        class="appearance-none border border-gray-300 rounded-md py-2 px-4 leading-tight focus:outline-none focus:border-blue-500">
                                    <p class="text-red-500 text-sm">{{ deliveryAddress.number.errorText }}</p>
                                </div>
                            </div>
                            <div class="col-span-full grid grid-cols-2 gap-4 mb-4">
                                <div class="flex flex-col gap-2">
                                    <label class="font-medium text-gray-700">Apartment</label>
                                    <input v-model="deliveryAddress.apartment.text" type="text"
                                        class="appearance-none border border-gray-300 rounded-md py-2 px-4 leading-tight focus:outline-none focus:border-blue-500">
                                </div>
                                <div class="flex flex-col gap-2">
                                    <label class="font-medium text-gray-700">City<span
                                            class="text-red-500">*</span></label>
                                    <input v-model="deliveryAddress.city.text" @blur="validateCity" type="text"
                                        class="appearance-none border border-gray-300 rounded-md py-2 px-4 leading-tight focus:outline-none focus:border-blue-500">
                                    <p class="text-red-500 text-sm">{{ deliveryAddress.city.errorText }}</p>
                                </div>
                            </div>
                            <div class="col-span-full grid grid-cols-2 gap-4 mb-4">
                                <div class="flex flex-col gap-2">
                                    <label class="font-medium text-gray-700">Floor</label>
                                    <input v-model="deliveryAddress.floor.text" type="text"
                                        class="appearance-none border border-gray-300 rounded-md py-2 px-4 leading-tight focus:outline-none focus:border-blue-500">
                                </div>
                                <div class="flex flex-col gap-2">
                                    <label class="font-medium text-gray-700">Entrance</label>
                                    <input v-model="deliveryAddress.entrance.text" type="text"
                                        class="appearance-none border border-gray-300 rounded-md py-2 px-4 leading-tight focus:outline-none focus:border-blue-500">
                                </div>
                            </div>
                            <div class="col-span-full grid grid-cols-2 gap-4 mb-4">
                                <div class="flex flex-col gap-2">
                                    <label class="font-medium text-gray-700">Add a note for delivery</label>
                                    <input v-model="deliveryAddress.note.text" type="text"
                                        class="appearance-none border border-gray-300 rounded-md py-2 px-4 leading-tight focus:outline-none focus:border-blue-500">
                                </div>
                            </div>
                            <h1 class="text-2xl font-bold my-8 col-span-full">Contact Details</h1>
                            <div class="col-span-full grid grid-cols-2 gap-4 mb-4">
                                <div class="flex flex-col gap-2">
                                    <label class="font-medium text-gray-700">Full Name<span
                                            class="text-red-500">*</span></label>
                                    <input v-model="contactDetails.fullName.text" @blur="validateFullName" type="text"
                                        class="appearance-none border border-gray-300 rounded-md py-2 px-4 leading-tight focus:outline-none focus:border-blue-500">
                                    <p class="text-red-500 text-sm">{{ contactDetails.fullName.errorText }}</p>
                                </div>
                                <div class="flex flex-col gap-2">
                                    <label class="font-medium text-gray-700">Email<span
                                            class="text-red-500">*</span></label>
                                    <input v-model="contactDetails.email.text" @blur="validateEmail" type="text"
                                        class="appearance-none border border-gray-300 rounded-md py-2 px-4 leading-tight focus:outline-none focus:border-blue-500">
                                    <p class="text-red-500 text-sm">{{ contactDetails.email.errorText }}</p>
                                </div>
                            </div>
                            <div class="col-span-full grid grid-cols-2 gap-4 mb-4">
                                <div class="flex flex-col gap-2">
                                    <label class="font-medium text-gray-700">Phone Number<span
                                            class="text-red-500">*</span></label>
                                    <input v-model="contactDetails.phoneNumber.text" @blur="validatePhoneNumber"
                                        type="text"
                                        class="appearance-none border border-gray-300 rounded-md py-2 px-4 leading-tight focus:outline-none focus:border-blue-500">
                                    <p class="text-red-500 text-sm">{{ contactDetails.phoneNumber.errorText }}</p>
                                </div>
                            </div>
                        </div>
                        <div
                            class="grid grid-cols-12 bg-white rounded-xl shadow-lg max-w-4xl text-gray-700 p-8 h-fit mt-8">
                            <h1 class="text-2xl font-bold mb-4 col-span-full">Payment</h1>
                            <div class="col-span-full mb-4">
                                <div class="flex flex-col gap-2">
                                    <!-- Checkboxes -->
                                    <div class="flex flex-row gap-2">
                                        <input type="radio" id="cashOnDelivery" name="paymentMethod"
                                            value="cashOnDelivery" checked>
                                        <label for="cashOnDelivery">Cash on Delivery</label>
                                    </div>
                                    <div class="flex flex-row gap-2 text-gray-500">
                                        <input type="radio" id="payWithCard" name="paymentMethod" value="payWithCard"
                                            disabled>
                                        <label for="payWithCard">Pay with Card (Planned)</label>
                                    </div>
                                </div>
                                <button @click="validateForm"
                                    class="rounded-2xl bg-white text-lg font-bold text-gray-700 mt-8 px-6 py-4 flex items-center gap-2 hover:bg-gray-200 border-2 border-gray-300">Complete
                                    Order with ${{ totalPrice.toFixed(2) }}
                                    <Icon class="text-2xl" name="material-symbols:select-check-box-rounded" />
                                </button>
                                <p class="text-red-500 text-sm ml-2 mt-2">{{ formErrorText }}</p>
                            </div>
                        </div>
                    </div>
                    <div
                        class="lg:block hidden col-span-4 h-fit bg-white rounded-xl shadow-lg max-w-4xl text-gray-700 p-8">
                        <h1 class="text-2xl font-bold mb-8">Your Cart</h1>
                        <div class="flex flex-col gap-2 mb-10 max-h-[50vh] overflow-y-auto">
                            <div v-for="(item, index) in items" :key="index"
                                class="grid grid-cols-12 gap-5 bg-white h-24 rounded-xl max-w-sm text-gray-700">
                                <img class="col-span-3 object-cover object-center w-36 h-[70px] rounded-xl"
                                    :src="'data:image/jpeg;base64,' + item.image">
                                <div class="col-span-9 flex flex-col pt-4 pb-2 px-4">
                                    <h1 class="text-lg font-semibold mb-2">{{ item.name }}</h1>
                                    <span class="flex justify-end mt-auto">
                                        <p class="text-xl font-bold">${{ item.price.toFixed(2) }}</p>
                                    </span>
                                </div>
                            </div>
                        </div>
                        <div class="font-bold text-xl w-fit ml-auto">Total: ${{ totalPrice.toFixed(2) }}</div>
                    </div>
                </div>
            </div>
        </section>
    </main>
</template>

<script setup>
import { storeToRefs } from 'pinia';
import { useUserStore } from '~/store/user';
import { useCartStore } from '~/store/cart';
import { onMounted } from 'vue';

const { items } = storeToRefs(useCartStore());
const { clearCart } = useCartStore();
const { personalData } = storeToRefs(useUserStore());

const totalPrice = computed(() => {
    return items.value.reduce((total, item) => total + item.price, 0);
});

const deliveryAddress = ref({
    street: { text: '', errorText: '' },
    number: { text: '', errorText: '' },
    apartment: { text: '', errorText: '' },
    city: { text: '', errorText: '' },
    floor: { text: '', errorText: '' },
    entrance: { text: '', errorText: '' },
    note: { text: '', errorText: '' }
});

const contactDetails = ref({
    fullName: { text: '', errorText: '' },
    email: { text: '', errorText: '' },
    phoneNumber: { text: '', errorText: '' }
});

onMounted(() => {
    deliveryAddress.value.street.text = personalData.value.road ? personalData.value.road : '';
    deliveryAddress.value.number.text = personalData.value.house_number ? personalData.value.house_number : '';
    deliveryAddress.value.city.text = personalData.value.city ? personalData.value.city : '';
});

const formErrorText = ref('');

const validateStreet = () => {
    if (!deliveryAddress.value.street.text.trim()) {
        deliveryAddress.value.street.errorText = '*This field is required';
    } else {
        deliveryAddress.value.street.errorText = '';
    }
}

const validateNumber = () => {
    const numberRegex = /^[0-9]+$/;

    if (!deliveryAddress.value.number.text.trim()) {
        deliveryAddress.value.number.errorText = '*This field is required';
    } else if (!numberRegex.test(deliveryAddress.value.number.text.trim())) {
        deliveryAddress.value.number.errorText = '*Please enter a valid number';
    } else {
        deliveryAddress.value.number.errorText = '';
    }
}

const validateCity = () => {
    if (!deliveryAddress.value.city.text.trim()) {
        deliveryAddress.value.city.errorText = '*This field is required';
    } else {
        deliveryAddress.value.city.errorText = '';
    }
}

const validateFullName = () => {
    if (!contactDetails.value.fullName.text.trim()) {
        contactDetails.value.fullName.errorText = '*This field is required';
    } else {
        contactDetails.value.fullName.errorText = '';
    }
}

const validateEmail = () => {
    const emailRegex = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;

    if (!contactDetails.value.email.text.trim()) {
        contactDetails.value.email.errorText = '*This field is required';
    } else if (!emailRegex.test(contactDetails.value.email.text.trim())) {
        contactDetails.value.email.errorText = '*Please enter a valid email address';
    } else {
        contactDetails.value.email.errorText = '';
    }
}

const validatePhoneNumber = () => {
    const phoneNumberRegex = /^[0-9]+$/;

    if (!contactDetails.value.phoneNumber.text.trim()) {
        contactDetails.value.phoneNumber.errorText = '*This field is required';
    } else if (!phoneNumberRegex.test(contactDetails.value.phoneNumber.text.trim())) {
        contactDetails.value.phoneNumber.errorText = '*Please enter a valid phone number';
    } else {
        contactDetails.value.phoneNumber.errorText = '';
    }
}

const computedAddress = computed(() => {
    return `${deliveryAddress.value.street.text} ${deliveryAddress.value.number.text}, ${deliveryAddress.value.city.text}`;
});

const addProduct = async () => {
    try {
        const encodedAddress = encodeURIComponent(computedAddress.value);
        const addressData = await $fetch(`https://nominatim.openstreetmap.org/search?format=json&q=${encodedAddress}&addressdetails=1&limit=1`);
        console.log(addressData);
        if (addressData[0]) {
            const { lat, lon } = addressData[0];
            console.log(lat, lon);

            const data = await $fetch('https://localhost:7230/api/orders', {
                method: 'post',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ products: items.value, address: computedAddress.value, latitude: lat, longitude: lon, fullName: contactDetails.value.fullName.text, phoneNumber: contactDetails.value.phoneNumber.text, email: contactDetails.value.email.text, floor: deliveryAddress.value.floor.text, apartment: deliveryAddress.value.apartment.text, entrance: deliveryAddress.value.entrance.text, deliveryNote: deliveryAddress.value.note.text}),
            });

            if (data) {
                // Handle success response 
                console.log('Order placed successfully!')
                clearCart();
                return navigateTo('/');
            } else {
                console.log('Something went wrong in the order placement!');
            }
        }
    } catch (error) {
        // Handle fetch error
        console.log('Something went wrong! ' + error);
    }
};

const validateForm = () => {
    validateStreet();
    validateNumber();
    validateCity();
    validateFullName();
    validateEmail();
    validatePhoneNumber();

    if (
        Object.values(deliveryAddress.value).every(field => field.errorText === '') &&
        Object.values(contactDetails.value).every(field => field.errorText === '')
    ) {
        addProduct();
    } else {
        formErrorText.value = '*Please meet all field requirements';
    }
};

</script>