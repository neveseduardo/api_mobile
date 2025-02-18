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
			const { data: { accessToken, refreshToken } } = await AuthService.login({ Email: username, Password: password });

			await AsyncStorage.setItem('access_token', accessToken);
			await AsyncStorage.setItem('refresh_token', refreshToken);

			setAccessToken(accessToken);
			setRefreshToken(refreshToken);

			const { data: user } = await AuthService.userData();

			setUserData(user);
			await AsyncStorage.setItem('userData', JSON.stringify(user));

			return { accessToken, user };
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