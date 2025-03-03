import { Stack } from 'expo-router';
import { StatusBar } from 'expo-status-bar';

export default function RootLayout() {
	return (
		<>
			<Stack initialRouteName="index">
				<Stack.Screen name="index" options={{
					headerShown: true,
					headerTitle: 'Unidades médicas',
				}}
				/>
			</Stack>
			<StatusBar style="auto" />
		</>
	);
}
