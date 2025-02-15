import { useState } from 'react';
import { TextInput, Button } from 'react-native';
import { router } from 'expo-router';
import { ThemedView } from '@/components/ThemedView';
import { ThemedText } from '@/components/ThemedText';

export default function RegisterScreen() {
	const [email, setEmail] = useState('');
	const [password, setPassword] = useState('');

	const handleRegister = () => {
		router.replace('/(auth)/login');
	};

	return (
		<ThemedView style={{ flex: 1, justifyContent: 'center', alignItems: 'center' }}>
			<ThemedText>Register</ThemedText>
			<TextInput
				placeholder="Email"
				value={email}
				onChangeText={setEmail}
				style={{ borderWidth: 1, padding: 10, margin: 10, width: 200 }}
			/>
			<TextInput
				placeholder="Password"
				value={password}
				onChangeText={setPassword}
				secureTextEntry
				style={{ borderWidth: 1, padding: 10, margin: 10, width: 200 }}
			/>
			<Button title="Register" onPress={handleRegister} />
			<Button title="Back to Login" onPress={() => router.back()} />
		</ThemedView>
	);
}