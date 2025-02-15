import { useState } from 'react';
import { TextInput, Button, StyleSheet, View } from 'react-native';
import { Href, router } from 'expo-router';
import { ThemedView } from '@/components/ThemedView';
import { ThemedText } from '@/components/ThemedText';

export default function LoginScreen() {
	const [email, setEmail] = useState('');
	const [password, setPassword] = useState('');

	const handleLogin = () => {
		router.replace("/(tabs)" as Href);
	};

	return (
		<ThemedView className="flex-1 items-center justify-center p-4">
			<View className="text-2xl w-full">
				AQUI TAILWIND
			</View>

			<View className="flex flex-col gap-4">
				<TextInput
					placeholder="Email"
					value={email}
					onChangeText={setEmail}
				/>
				<TextInput
					placeholder="Password"
					value={password}
					onChangeText={setPassword}
					secureTextEntry
				/>

				<Button title="Login" onPress={handleLogin} />
				<Button title="Register" onPress={() => router.push('/(auth)/register')} />
			</View>




		</ThemedView>
	);
}

const styles = StyleSheet.create({
	container: {
		flex: 1,
		justifyContent: 'center',
		alignItems: 'center',
	},
});