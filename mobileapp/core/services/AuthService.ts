import { IAuthService } from '../ports/IAuthService';
import { HttpClient } from '../ports/IHttpClient';

export class AuthService implements IAuthService {
	private client: HttpClient;

	constructor(client: HttpClient) {
		this.client = client;
	}

	async login(data: any): Promise<any> {
		return this.client.post('/auth/login', data);
	}

	async userData() {
		return this.client.get('/auth/usuario');
	}

	async register(data: any) {
		return this.client.post('/auth/register', data);
	}

	async logout() {
		return this.client.post('/auth/logout');
	}
};