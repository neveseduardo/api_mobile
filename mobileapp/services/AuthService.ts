import { HttpClient } from './IHttpClient';
import { IUser } from './IUser';

interface IAuthService {
	login: (data: any) => Promise<{ accessToken: string, refreshToken: string }>,
	userData: () => Promise<IUser>,
	register: (data: { name: string, cpf: string, email: string, password: string }) => Promise<any>,
	logout: () => Promise<any>,
}

class AuthService implements IAuthService {
	private client: HttpClient;

	constructor(client: HttpClient) {
		this.client = client;
	}

	async login(payload: any): Promise<{ accessToken: string, refreshToken: string }> {
		try {
			const { data: response } = await this.client.post('/auth/login', {
				Email: payload.username,
				Password: payload.password,
			});

			if (!response?.data) {
				throw 'Dados não encontrados';
			}

			const { accessToken, refreshToken } = response?.data;

			if (!accessToken || !refreshToken) {
				throw 'Tokens auxentes';
			}

			const tokens = {
				accessToken: accessToken as string,
				refreshToken: refreshToken as string,
			};

			if (!tokens.accessToken || !tokens.refreshToken) {
				throw 'Não autorizado!';
			}

			return tokens;
		} catch (error) {
			console.error(error);
			throw error;
		}
	}

	async userData() {
		try {
			const response = await this.client.get('/auth/usuario');

			const user = response?.data?.data;

			if (!user) {
				throw 'Usuário não encontrado';
			}

			const userObject: IUser = {
				id: user.id,
				email: user.email,
				name: user.name,
			};

			return userObject;
		} catch (error) {
			throw error;
		}

	}

	async register(data: any) {
		return this.client.post('/auth/register', data);
	}

	async logout() {
		return this.client.post('/auth/logout');
	}
};

export const useAuthService = (client: HttpClient) => {
	const service = new AuthService(client);

	return { service };
};