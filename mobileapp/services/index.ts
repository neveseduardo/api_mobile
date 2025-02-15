import axios, { type AxiosInstance, type InternalAxiosRequestConfig, AxiosError } from 'axios';

const http: AxiosInstance = axios.create({
	baseURL: process.env.EXPO_PUBLIC_API_URL ?? '/',
	timeout: 3 * 60 * 1000,
});

http.interceptors.request.use((config: InternalAxiosRequestConfig): InternalAxiosRequestConfig => {
	if (config.headers['Content-Type'] !== 'multipart/form-data') {
		config.headers['Content-Type'] = 'application/json';
		config.headers['Accept'] = 'application/json';
	}
	return config;
});

export default http;