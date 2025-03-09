import { Stack } from 'expo-router';

export default function RootLayout() {

	return (
		<Stack initialRouteName="index">
			<Stack.Screen name="index" options={{ headerShown: true, title: 'Meus dados' }} />
		</Stack>
	);
}