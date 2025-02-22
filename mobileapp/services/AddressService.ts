import { HttpClient } from './IHttpClient';

interface IAddressService {
	search: (data: any) => Promise<{
		logradouro: string,
		bairro: string,
		cidade: string,
		estado: string,
	}>,
	estados: () => Promise<any[]>,
	createAndBindUser: (data: any, id: number) => Promise<{ addressId: number }>
}

class AddressService implements IAddressService {
	private client: HttpClient;

	constructor(client: HttpClient) {
		this.client = client;
	}

	/**
	 * Pesquisa endereco pelo CEP
	 * @param cep
	 * @returns
	 */
	async search(cep: string): Promise<{
		logradouro: string,
		bairro: string,
		cidade: string,
		estado: string,
	}> {
		try {
			const API_URL = process.env.EXPO_PUBLIC_APICEP_URL ?? '';
			const REPLACED = API_URL?.replace('{cep}', cep);
			const response = await this.client.get(REPLACED);

			const data = {
				logradouro: response.data.logradouro ?? '',
				bairro: response.data.bairro ?? '',
				cidade: response.data.localidade ?? '',
				estado: response.data.uf ?? '',
			};

			return data;
		} catch (error) {
			console.error(error);
			throw error;
		}
	}

	/**
	 * Retorna lista de estados brasileiros
	 * @returns
	 */
	async estados() {
		try {
			const API_URL = process.env.EXPO_PUBLIC_IBGE_URL ?? '';
			const response = await this.client.get(`${API_URL}/localidades/estados`);
			return response.data;
		} catch (error) {
			console.error(error);
			throw error;
		}
	}

	/**
	 * Cria endereço e vincula automaticamente ao usuário autenticado
	 *
	 * @param data
	 * @param id
	 * @returns
	 */
	async createAndBindUser(data: any, id: number) {
		try {
			const response = await this.client.post(`/auth/endereco/${id}`, data);

			return { addressId: response.data.Id };
		} catch (error) {
			console.error(error);
			throw error;
		}
	}
};

export const useAddressService = (client: HttpClient) => {
	const service = new AddressService(client);

	return { service };
};
