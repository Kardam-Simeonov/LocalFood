import { defineStore } from 'pinia';

interface UserPayloadInterface {
  email: string;
  password: string;
}

export const useAuthStore = defineStore('auth', {
  state: () => ({
    authenticated: false,
    userEmail: null,
    loading: false,
  }),
  actions: {
    async authenticateUser({ email, password }: UserPayloadInterface) {
      // Set loading to true
      this.loading = true;

      console.log(JSON.stringify({ email, password }))
      try {
        const data : any = await $fetch('https://localhost:7230/api/auth/login', {
          method: 'post',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify({ email, password }), // Ensure body is stringified
        });

        if (data) {
          console.log(data)
          const token = useCookie('token'); 
          token.value = data; 
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