import { DarkTheme, DefaultTheme, ThemeProvider } from '@react-navigation/native';
import { Stack } from 'expo-router';
import { StatusBar } from 'expo-status-bar';
import { useColorScheme } from '@/hooks/useColorScheme';


export default function AddressLayout() {
	const colorScheme = useColorScheme();

	return (
		<ThemeProvider value={colorScheme === 'dark' ? DarkTheme : DefaultTheme}>
			<Stack initialRouteName="index">
				<Stack.Screen name="index" options={{ headerShown: true, headerTitle: 'Meus endereços' }} />
				<Stack.Screen name="postalcode" options={{ headerShown: true, headerTitle: 'Buscar CEP' }} />
				<Stack.Screen name="address" options={{ headerShown: true, headerTitle: 'Cadastrar endereço' }} />
			</Stack>
			<StatusBar style="auto" />
		</ThemeProvider>
	);
}
