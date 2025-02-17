import { DarkTheme, DefaultTheme, ThemeProvider } from '@react-navigation/native';
import { Stack } from 'expo-router';
import { useColorScheme } from '@/hooks/useColorScheme';
import { StatusBar } from 'expo-status-bar';
import { AuthProvider } from '@/contexts/AuthContext';
import '@/styles/global.css';

export default function AuthLayout() {
	const colorScheme = useColorScheme();

	return (
		<AuthProvider>
			<ThemeProvider value={colorScheme === 'dark' ? DarkTheme : DefaultTheme}>
				<Stack initialRouteName="login">
					<Stack.Screen name="login" options={{ headerShown: false }} />
					<Stack.Screen name="register" options={{ headerShown: false }} />
				</Stack>
				<StatusBar style="auto" />
			</ThemeProvider>
		</AuthProvider>
	);
}