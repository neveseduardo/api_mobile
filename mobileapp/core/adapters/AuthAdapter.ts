import { IAuthService } from '../ports/IAuthService';
import { IUser } from '../ports/IUser';

export interface IAuthadapter {
	login: (data: any) => Promise<{ accessToken: string, refreshToken: string }>,
	userData: () => Promise<IUser>,
	register: (data: { name: string, cpf: string, email: string, password: string }) => Promise<any>,
	logout: () => Promise<any>,
}

export class AuthAdapter implements IAuthadapter {
	private service: IAuthService;

	constructor(service: IAuthService) {
		this.service = service;
	}

	async login(payload: any): Promise<{ accessToken: string, refreshToken: string }> {
		try {
			const response = await this.service.login({
				Email: payload.username,
				Password: payload.password,
			});

			const tokens = {
				accessToken: (response.data?.accessToken ?? '') as string,
				refreshToken: (response.data?.refreshToken ?? '') as string,
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
			const { data } = await this.service.userData();

			if (!data?.email) {
				throw 'Erro ao capturar dados de usuário';
			}

			const user: IUser = {
				id: data.id,
				email: data.name,
				name: data.email,
			};

			return user;
		} catch (error) {
			throw error;
		}

	}

	async register(data: { name: string, cpf: string, email: string, password: string }) {
		//
	}

	async logout() {
		//
	}
}