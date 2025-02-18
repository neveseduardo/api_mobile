import { useState } from 'react';
import { View, Text } from 'react-native';
import { Href, router } from 'expo-router';
import { ThemedView } from '@/components/ThemedView';
import Button from '@/components/ui/Button';
import TextInput from '@/components/ui/TextInput';
import PasswordInput from '@/components/ui/PasswordInput';
import { ThemedText } from '@/components/ThemedText';
import Ionicons from '@expo/vector-icons/Ionicons';
import { useAuth } from '@/contexts/AuthContext';
import { AuthService } from '@/services/AuthService';
import AuthHeader from '@/components/auth/AuthHeader';

export default function LoginScreen() {
	const [name, setName] = useState('Username');
	const [cpf, setCPF] = useState('085.195.580-03');
	const [email, setEmail] = useState('email@email.com');
	const [password, setPassword] = useState('Senh@123');
	const [cpassword, setCPassword] = useState('Senh@123');
	const [loading, setLoading] = useState(false);

	const { login } = useAuth();

	const handleLogin = async () => {
		try {
			setLoading(true);

			await AuthService.register({ name, email, cpf, password });

			const { access_token, userData } = await login(email, password);

			if (access_token && userData) {
				router.replace('/(tabs)' as Href);
			}
		} catch (error) {
			console.error(error, email, password);
		} finally {
			setLoading(false);
		}
	};

	return (
		<ThemedView className="items-center justify-center flex-1 p-8 bg-gray-500">
			<View className="flex flex-col w-full gap-5">
				<AuthHeader
					title="Cadastre-se"
					description="Insira os dados solicitados abaixo para registrar-se no aplicativo."
				/>

				<View className="flex flex-col w-full gap-4">
					<TextInput
						autoCapitalize="words"
						returnKeyType="next"
						placeholder="Nome"
						value={name}
						onChangeText={setName}
						disabled={loading}
					/>
					<TextInput
						returnKeyType="next"
						placeholder="CPF"
						value={cpf}
						onChangeText={setCPF}
						disabled={loading}
					/>
					<TextInput
						keyboardType="email-address"
						returnKeyType="next"
						placeholder="Email"
						value={email}
						onChangeText={setEmail}
						disabled={loading}
					/>
					<PasswordInput
						placeholder="Senha"
						returnKeyType="next"
						value={password}
						onChangeText={setPassword}
						disabled={loading}
					/>
					<PasswordInput
						placeholder="Confirmar senha"
						returnKeyType="done"
						value={cpassword}
						onChangeText={setCPassword}
						disabled={loading}
					/>

					<Button
						onPress={handleLogin}
						color="primary"
						className="w-full"
						disabled={loading}
					>
						<Text className="text-white">REGISTRAR</Text>
					</Button>

					<Button
						onPress={() => router.push('/(auth)/login')}
						className="w-full"
						disabled={loading}
					>
						<ThemedText>LOGIN</ThemedText>
					</Button>
				</View>
			</View>
		</ThemedView>
	);
}