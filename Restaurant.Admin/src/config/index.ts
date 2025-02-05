import axios from 'axios';

export const BASE_API_URL = import.meta.env.VITE_BASE_API_URL;
export const DEVELOPMENT = import.meta.env.DEV;

export const httpClient = axios.create({
	baseURL: BASE_API_URL,
});
