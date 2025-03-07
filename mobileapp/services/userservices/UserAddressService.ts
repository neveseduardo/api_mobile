import { HttpClient } from '@/@types/IHttpClient';

export class UserAddressService {
	private client: HttpClient;

	constructor(client: HttpClient) {
		this.client = client;
	}

	async search(cep: string): Promise<any> {
		const API_URL = process.env.EXPO_PUBLIC_APICEP_URL ?? '';
		const REPLACED = API_URL?.replace('{cep}', cep);
		return this.client.get(REPLACED);
	}

	async addFromUserAsync(data: any): Promise<any> {
		return this.client.post('/public/endereco', data);
	}

	async getAllFromUserAsync(): Promise<any> {
		return this.client.get('/public/endereco');
	}

	async getByIdFromUserAsync(id: number): Promise<any> {
		return this.client.get('/public/endereco');
	}

	async updateFromUserAsync(id: number, data: any) {
		return this.client.put('/public/endereco', data);
	}

	async deleteFromUserAsync(id: number) {
		return this.client.delete('/public/endereco');
	}
};