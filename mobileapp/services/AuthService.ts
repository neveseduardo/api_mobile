import HttpClient from '@/services';

export const AuthService = {
	login: <T = any>(data: any) => HttpClient.post<T>('/auth/login', data),
	userData: <T = any>() => HttpClient.get<T>('/auth/customer'),
	register: <T = any>(data: any) => HttpClient.post<T>('/auth/register', data),
	logout: <T = any>() => HttpClient.post<T>('/auth/logout'),
};