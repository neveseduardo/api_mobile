import { DarkTheme, DefaultTheme, ThemeProvider } from '@react-navigation/native';
import { Stack } from 'expo-router';
import { StatusBar } from 'expo-status-bar';
import { useColorScheme } from '@/hooks/useColorScheme';
import Ionicons from '@expo/vector-icons/Ionicons';
import Button from '../../../components/ui/Button';


export default function RootLayout() {
	const colorScheme = useColorScheme();

	return (
		<ThemeProvider value={colorScheme === 'dark' ? DarkTheme : DefaultTheme}>
			<Stack initialRouteName="index">
				<Stack.Screen name="index" options={{
					headerShown: true,
					headerTitle: 'Meu perfil',
					headerRight: ({ tintColor }) => (
						<Button className="mr-[16px] w-[40px] !bg-transparent" circular onPress={() => { }}>
							<Ionicons name="settings-outline" size={18} color={tintColor} />
						</Button>
					),
				}}
				/>
				<Stack.Screen name="address" options={{ headerShown: false, title: 'Meus endereços' }} />
			</Stack>
			<StatusBar style="auto" />
		</ThemeProvider>
	);
}
