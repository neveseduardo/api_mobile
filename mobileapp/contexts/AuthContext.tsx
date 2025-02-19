import { createContext, useState, useContext, ReactNode, useEffect } from 'react';
import AsyncStorage from '@react-native-async-storage/async-storage';
import { AuthAdapter } from '@/core/adapters/AuthAdapter';
import { AuthService } from '@/core/services/AuthService';
import { AxiosHttpClient } from '@/core/services/AxiosHttpClient';
import { IUser } from '@/core/ports/IUser';
import { Href, router } from 'expo-router';

interface AuthContextType {
	access_token: string;
	refresh_token: string;
	userData: IUser | null;
	login: (username: string, password: string) => Promise<any>;
	register: (name: string, email: string, cpf: string, password: string, addressId?: number) => Promise<any>;
	logout: () => Promise<any>;
}

interface AuthProviderProps {
	children: ReactNode;
}

const defaultValue: AuthContextType = {
	access_token: '',
	refresh_token: '',
	userData: null,
	login: async () => { },
	register: async () => { },
	logout: async () => { },
};

const AuthContext = createContext<AuthContextType>(defaultValue);

export const AuthProvider = ({ children }: AuthProviderProps) => {
	const [access_token, setAccessToken] = useState<string>(defaultValue.access_token);
	const [refresh_token, setRefreshToken] = useState<string>(defaultValue.refresh_token);
	const [userData, setUserData] = useState<IUser | null>(defaultValue.userData);
	const client = AxiosHttpClient;
	const service = new AuthService(client);
	const adapter = new AuthAdapter(service);

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
			const { accessToken, refreshToken } = await adapter.login({ username, password });

			await AsyncStorage.setItem('access_token', accessToken);
			await AsyncStorage.setItem('refresh_token', refreshToken);

			setAccessToken(accessToken);
			setRefreshToken(refreshToken);

			const user = await adapter.userData();

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
			await adapter.register({ name, email, cpf, password, addressId });
		} catch (error) {
			console.error('Erro ao fazer login:', error);
			throw error;
		}
	};

	const logout = async () => {
		try {
			await adapter.logout();

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
		<AuthContext.Provider
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
		</AuthContext.Provider>
	);
};

export const useAuth = () => useContext(AuthContext);