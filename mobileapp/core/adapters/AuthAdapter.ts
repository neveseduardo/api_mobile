import { IAuthService } from '../ports/IAuthService';
import { IUser } from '../ports/IUser';

export interface IAuthadapter {
	login: (data: any) => Promise<{ accessToken: string, refreshToken: string }>,
	userData: () => Promise<IUser>,
	register: (data: { name: string, cpf: string, email: string, password: string }) => Promise<any>,
	logout: () => Promise<any>,
}

/**
 * A função do adapter é adaptar o retorno das requisições para o que de fato vai entrar na aplicação
 * Caso algum dados na aplicação esteja inconsistente, o provável culpado é esse componente adapter ou o servidor
 *
 * @class AuthAdapter
 * @implements IAuthAdapter
 */
export class AuthAdapter implements IAuthadapter {
	private service: IAuthService;

	constructor(service: IAuthService) {
		this.service = service;
	}

	/**
	 * Função que autentica no servidor e retorna os tokens de autenticação
	 * Os tokens devem ser salvos em asyncStorage e utilzados para fazer as requisições do sistema
	 *
	 * @param payload
	 * @returns {
	 * 	accessToken: string,
	 * 	refreshToken: string
	 * }
	 */
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


	/**
	 * Função requisita os dados do usuário logado
	 * Esta função precisa de um token de autenticação valido, senão retorna erro 401 (Não autorizado!)
	 *
	 * @returns user: IUser
	 */
	async userData() {
		try {
			const { data } = await this.service.userData();

			if (!data?.email) {
				throw 'Erro ao capturar dados de usuário';
			}

			const user: IUser = {
				id: data.id,
				email: data.email,
				name: data.name,
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
		await this.service.logout();
	}
}