export interface IAddressService {
	search: <T = any>(cep: any) => Promise<T>,
	estados: <T = any>() => Promise<T>,
};