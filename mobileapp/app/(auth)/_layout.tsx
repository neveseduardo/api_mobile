import { Stack } from 'expo-router';
import { StatusBar } from 'expo-status-bar';
import { AuthProvider as UserAuthenticationProvider } from '@/contexts/UserAuthenticationContext';
import { AuthProvider as AdminAuthenticationProvider } from '@/contexts/AdminAuthenticationContext';
import '@/assets/styles/global.css';

export default function AuthLayout() {

	return (
		<AdminAuthenticationProvider>
			<UserAuthenticationProvider>
				<Stack initialRouteName="userlogin">
					<Stack.Screen name="userlogin" options={{ headerShown: false }} />
					<Stack.Screen name="register" options={{ headerShown: false }} />
					<Stack.Screen name="adminlogin" options={{ headerShown: false }} />
				</Stack>
				<StatusBar style="auto" />
			</UserAuthenticationProvider>
		</AdminAuthenticationProvider>
	);
}