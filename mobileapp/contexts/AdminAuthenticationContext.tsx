import { createContext, useState, useContext, useEffect } from 'react';
import AsyncStorage from '@react-native-async-storage/async-storage';
import { AxiosHttpClient } from '../services/AxiosHttpClient';
import { AuthContextType, AuthProviderProps, IAdmin } from '@/@types';
import { AdminAuthenticationService } from '@/services/AdminAuthenticationService';

const defaultValue: AuthContextType<IAdmin> = {
	access_token: '',
	refresh_token: '',
	userData: null,
	login: async () => { },
	register: async () => { },
	logout: async () => { },
};

const UserAuthenticationContext = createContext<AuthContextType<IAdmin>>(defaultValue);

export const AuthProvider = ({ children }: AuthProviderProps) => {
	const [access_token, setAccessToken] = useState<string>(defaultValue.access_token);
	const [refresh_token, setRefreshToken] = useState<string>(defaultValue.refresh_token);
	const [userData, setUserData] = useState<IAdmin | null>(defaultValue.userData);
	const httpClient = AxiosHttpClient;
	const service = new AdminAuthenticationService(httpClient);

	useEffect(() => {
		const loadAuthData = async () => {
			const storedAccessToken = await AsyncStorage.getItem('access_token');
			const storedRefreshToken = await AsyncStorage.getItem('refresh_token');
			const storedUserData = await AsyncStorage.getItem('userData');

			if (storedAccessToken && storedRefreshToken && storedUserData) {
				setAccessToken(storedAccessToken);
				setRefreshToken(storedRefreshToken);
				setUserData(JSON.parse(storedUserData));
			}
		};

		loadAuthData();
	}, []);

	const login = async (username: string, password: string) => {
		try {
			const { accessToken, refreshToken } = await service.login({ username, password });

			await AsyncStorage.setItem('access_token', accessToken);
			await AsyncStorage.setItem('refresh_token', refreshToken);

			setAccessToken(accessToken);
			setRefreshToken(refreshToken);

			const user = await service.userData();

			setUserData(user);
			await AsyncStorage.setItem('userData', JSON.stringify(user));

			return { accessToken, user };
		} catch (error) {
			console.error('Erro ao fazer login:', error);
			throw error;
		}
	};

	const register = async (name: string, email: string, cpf: string, password: string, addressId?: number) => {
		try {
			await service.register({ name, email, cpf, password, addressId });
		} catch (error) {
			console.error('Erro ao fazer login:', error);
			throw error;
		}
	};

	const logout = async () => {
		try {
			await service.logout();

			setAccessToken('');
			setRefreshToken('');
			setUserData(null);

			await AsyncStorage.removeItem('access_token');
			await AsyncStorage.removeItem('refresh_token');
			await AsyncStorage.removeItem('userData');
		} catch (error) {
			console.error('Erro ao fazer logout:', error);
			throw error;
		}
	};

	return (
		<UserAuthenticationContext.Provider
			value={{
				access_token,
				refresh_token,
				userData,
				register,
				login,
				logout,
			}}
		>
			{children}
		</UserAuthenticationContext.Provider>
	);
};

export const useAdminAuth = () => useContext(UserAuthenticationContext);