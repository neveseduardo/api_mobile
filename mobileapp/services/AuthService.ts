export const AuthService = {
	signin: async (username: string, password: string): Promise<any> => {
		return new Promise((resolve, reject) => {
			setTimeout(() => {
				if (username === 'email@email.com' && password === 'Senh@123') {
					resolve({
						access_token: 'fake-token',
						refresh_token: 'fake-token',
					});
				} else {
					reject(new Error('Credenciais inválidas'));
				}
			}, 1000);
		});
	},
	user: async () => {
		return new Promise((resolve) => {
			setTimeout(() => {
				resolve({
					id: 1,
					username: 'user',
					email: 'user@example.com',
				});
			}, 1000);
		});
	},
	signup: async (data: any) => {
		return new Promise((resolve) => {
			setTimeout(() => {
				resolve({
					id: 1,
					username: 'user',
					email: 'user@example.com',
				});
			}, 1000);
		});
	},
	logout: async () => {
		return new Promise((resolve) => setTimeout(() => resolve, 1000));
	},
};