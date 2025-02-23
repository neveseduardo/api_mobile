import { IAddress } from './IAddress';
import { HttpClient } from './IHttpClient';

interface IAddressService {
	search: (data: any) => Promise<{
		logradouro: string,
		bairro: string,
		cidade: string,
		estado: string,
	}>,
	estados: () => Promise<any[]>,
	createAndBindUser: (data: any, id: number) => Promise<{ addressId: number }>,
	getAddresses: (id: number) => Promise<IAddress[]>,
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
			const response = await this.client.post(`/auth/usuarios/${id}/enderecos`, data);

			return { addressId: response.data.Id };
		} catch (error) {
			console.error(error);
			throw error;
		}
	}

	async getAddresses(id: number) {
		try {
			const response = await this.client.get(`/auth/usuarios/${id}/enderecos`);

			if (Array.isArray(response?.data?.data)) {
				return response?.data?.data as IAddress[];
			}

			return [];
		} catch (error) {
			console.error(error);
			throw 'Erro ao pegar lista de endereços';
		}
	}
};

export const useAddressService = (client: HttpClient) => {
	const service = new AddressService(client);

	return { service };
};
