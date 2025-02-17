import { createContext, useState, useContext, ReactNode, useEffect } from 'react';
import AsyncStorage from '@react-native-async-storage/async-storage';
import { AuthService } from '@/services/AuthService';

interface UserData {
	id: number;
	username: string;
	email: string;
}

interface AuthContextType {
	access_token: string;
	refresh_token: string;
	userData: UserData | null;
	login: (username: string, password: string) => Promise<any>;
	logout: () => Promise<void>;
}

interface AuthProviderProps {
	children: ReactNode;
}

const defaultValue: AuthContextType = {
	access_token: '',
	refresh_token: '',
	userData: null,
	login: async () => { },
	logout: async () => { },
};

const AuthContext = createContext<AuthContextType>(defaultValue);

export const AuthProvider = ({ children }: AuthProviderProps) => {
	const [access_token, setAccessToken] = useState<string>(defaultValue.access_token);
	const [refresh_token, setRefreshToken] = useState<string>(defaultValue.refresh_token);
	const [userData, setUserData] = useState<UserData | null>(defaultValue.userData);

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
			const { access_token, refresh_token } = await AuthService.signin(username, password);
			const user = await AuthService.user();

			setAccessToken(access_token);
			setRefreshToken(refresh_token);
			setUserData(user as UserData);

			await AsyncStorage.setItem('access_token', access_token);
			await AsyncStorage.setItem('refresh_token', refresh_token);
			await AsyncStorage.setItem('userData', JSON.stringify(user));

			return { access_token, userData };
		} catch (error) {
			console.error('Erro ao fazer login:', error);
			throw error;
		}
	};

	const logout = async () => {
		try {
			await AuthService.logout();

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
		<AuthContext.Provider value={{ access_token, refresh_token, userData, login, logout }}>
			{children}
		</AuthContext.Provider>
	);
};

export const useAuth = () => useContext(AuthContext);