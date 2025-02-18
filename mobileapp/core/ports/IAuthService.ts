export interface IAuthService {
	login: <T = any>(data: any) => Promise<T>,
	userData: <T = any>() => Promise<T>,
	register: <T = any>(data: any) => Promise<T>,
	logout: <T = any>() => Promise<T>,
};