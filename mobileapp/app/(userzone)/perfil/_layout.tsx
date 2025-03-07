import { Stack } from 'expo-router';
import Ionicons from '@expo/vector-icons/Ionicons';
import Button from '@/components/ui/Button';


export default function RootLayout() {
	return (
		<Stack initialRouteName="index">
			<Stack.Screen
				name="index"
				options={{
					headerShown: true,
					headerTitle: 'Meu perfil',
					headerRight: ({ tintColor }) => (
						<Button className="mr-[16px] w-[40px] !bg-transparent" circular onPress={() => { }}>
							<Ionicons name="settings-outline" size={18} color={tintColor} />
						</Button>
					),
				}}
			/>
			<Stack.Screen name="enderecos" options={{ headerShown: false }} />
		</Stack>
	);
}
