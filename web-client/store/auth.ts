import { defineStore } from 'pinia';

interface UserPayloadInterface {
  username: string;
  password: string;
}

export const useAuthStore = defineStore('auth', {
  state: () => ({
    authenticated: false,
    loading: false,
  }),
  actions: {
    async authenticateUser({ username, password }: UserPayloadInterface) {
      // Set loading to true
      this.loading = true;

      try {
        const data : any = await $fetch('https://dummyjson.com/auth/login', {
          method: 'post',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify({ username, password }), // Ensure body is stringified
        });

        if (data) {
          const token = useCookie('token'); 
          token.value = data?.token; 
          this.authenticated = true;
        } else {
          // Handle error response
          console.error('Authentication failed');
        }
      } catch (error) {
        // Handle fetch error
        console.error('Error during authentication:', error);
      } finally {
        // Set loading to false
        this.loading = false;
      }
    },
    logUserOut() {
      const token = useCookie('token'); 
      this.authenticated = false; 
      token.value = null; 
    },
  },
});