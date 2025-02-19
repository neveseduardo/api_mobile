import { IAddressService } from '../ports/IAddressService';
import { HttpClient } from '../ports/IHttpClient';

export class AddressService implements IAddressService {
	private client: HttpClient;

	constructor(client: HttpClient) {
		this.client = client;
	}

	async search(cep: string): Promise<any> {
		const API_URL = process.env.EXPO_PUBLIC_APICEP_URL ?? '';
		const REPLACED = API_URL?.replace('{cep}', cep);
		return this.client.get(REPLACED);
	}

	async estados(): Promise<any> {
		const API_URL = process.env.EXPO_PUBLIC_IBGE_URL ?? '';
		return this.client.get(`${API_URL}/localidades/estados`);
	}
};