import { useState } from 'react';
import { TextInput, Button, StyleSheet } from 'react-native';
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
		<ThemedView style={styles.container}>
			<ThemedText>Login</ThemedText>
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
			<Button title="Login" onPress={handleLogin} />
			<Button title="Register" onPress={() => router.push('/(auth)/register')} />
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