import axios, { type AxiosInstance, type InternalAxiosRequestConfig } from 'axios';
import AsyncStorage from '@react-native-async-storage/async-storage';


const HttpClient: AxiosInstance = axios.create({
	baseURL: process.env.EXPO_PUBLIC_API_URL ?? '/',
	timeout: 3 * 60 * 1000,
});

HttpClient.interceptors.request.use(async (config: InternalAxiosRequestConfig): Promise<InternalAxiosRequestConfig> => {
	if (config.headers['Content-Type'] !== 'multipart/form-data') {
		config.headers['Content-Type'] = 'application/json';
		config.headers['Accept'] = 'application/json';
	}

	const token = await AsyncStorage.getItem('access_token');

	if (token) {
		config.headers['Authorization'] = `Bearer ${token}`;
	}

	return config;
}, error => {
	return Promise.reject(error);
});

export default HttpClient;